using FluentResults;
using LightCap.InvestmentApi.Application.Common.Interfaces;
using LightCap.InvestmentApi.Application.Features.MonoService.MonoDebit;
using LightCap.InvestmentApi.Domain.Entities;
using LightCap.InvestmentApi.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LightCap.InvestmentApi.Application.Features.MonoService.Transaction.Commands
{
    public record ProcessMonoTransactionCommand(MonoTransactionWebhookPayload Payload) : IRequest<Result<ProcessMonoTransactionResponse>>;

    public class ProcessMonoTransactionHandler(
        IRepository<LinkedBankAccount> linkedAccountRepository,
        IRepository<User> userRepository,
        IRepository<Wallet> walletRepository,
        IRepository<WalletTransaction> walletTransactionRepository,
        IRepository<DirectDebitMandate> mandateRepository,
        IMonoDebitService monoDebitService
    ) : IRequestHandler<ProcessMonoTransactionCommand, Result<ProcessMonoTransactionResponse>>
    {
        // Mono's hard floor - a debit below this is rejected outright by their API.
        // Below this, LightCap debits ₦200 flat instead (disclosed in the user agreement).
        // At or above this, the exact calculated percentage is debited.
        private const decimal MinimumDebitAmount = 200m;

        public async Task<Result<ProcessMonoTransactionResponse>> Handle(ProcessMonoTransactionCommand request,
            CancellationToken cancellationToken)
        {
            var data = request.Payload.Data;

            // 1. Only debit (spend) transactions trigger a round-up.
            if (!string.Equals(data.Type, "debit", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Ok(new ProcessMonoTransactionResponse
                {
                    RoundUpApplied = false,
                    Message = "Not a debit transaction - ignored."
                });
            }

            // 2. Idempotency guard - stop a retried webhook double-processing the same spend.
            var alreadyProcessed = walletTransactionRepository.Exists(x => x.SourceReference == data.Id);

            if (alreadyProcessed)
            {
                return Result.Ok(new ProcessMonoTransactionResponse
                {
                    RoundUpApplied = false,
                    Message = "Transaction already processed - ignored duplicate webhook."
                });
            }

            // 3. Find which LightCap user this Mono account belongs to.
            var linkedAccount = await linkedAccountRepository.GetSingleAsync(
                x => x.MonoAccountId == data.Account && x.IsActive);

            if (linkedAccount == null)
            {
                return Result.Fail("No active linked account found for this Mono account ID.");
            }

            var user = await userRepository.GetByIdAsync(linkedAccount.UserId);

            if (user == null)
            {
                return Result.Fail("Linked account exists but its user could not be found.");
            }

            // 4. Calculate the round-up, then apply the ₦200 floor rule:
            //    - below ₦200 calculated -> debit ₦200 flat
            //    - ₦200 or above -> debit the exact calculated amount
            var calculatedRoundUp = data.Amount * (user.AutoInvestPercentage / 100m);
            var debitAmount = calculatedRoundUp < MinimumDebitAmount ? MinimumDebitAmount : calculatedRoundUp;

            // 5. Fetch (or create) their wallet.
            var wallet = await walletRepository.GetSingleAsync(x => x.UserId == user.Id);

            if (wallet == null)
            {
                wallet = new Wallet
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    PendingRoundUpBalance = 0,
                    AvailableBalance = 0,
                    TotalInvested = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await walletRepository.AddAsync(wallet, cancellationToken);
            }

            // 6. Confirm the user's Direct Debit mandate is actually active.
            var mandate = await mandateRepository.GetSingleAsync(
                x => x.UserId == user.Id && x.Status == MandateStatus.ReadyToDebit);

            if (mandate == null)
            {
                // FALLBACK: mandate not ready yet - track virtually instead of debiting.
                wallet.PendingRoundUpBalance += calculatedRoundUp;
                wallet.UpdatedAt = DateTime.UtcNow;

                var fallbackTransaction = new WalletTransaction
                {
                    Id = Guid.NewGuid(),
                    WalletId = wallet.Id,
                    UserId = user.Id,
                    Type = WalletTransactionType.SpendRoundUp,
                    Status = WalletTransactionStatus.Failed,
                    Amount = 0,
                    AvailableBalanceAfter = wallet.AvailableBalance,
                    SourceReference = data.Id,
                    Description = $"Mandate not ready - \u20a6{calculatedRoundUp} tracked as pending, not debited.",
                    CreatedAt = DateTime.UtcNow
                };

                await walletTransactionRepository.AddAsync(fallbackTransaction, cancellationToken);
                await walletRepository.UpdateAsync(wallet);
                await walletRepository.SaveChanges(cancellationToken);

                return Result.Ok(new ProcessMonoTransactionResponse
                {
                    RoundUpApplied = false,
                    Message = "Mandate not yet active - round-up tracked but not debited."
                });
            }

            // 7. Mandate is ready - attempt the real debit now.
            var debitReference = $"roundup-{data.Id}-{Guid.NewGuid():N}";

            var debitResult = await monoDebitService.DebitAsync(new MonoDebitRequest
            {
                MonoMandateId = mandate.MonoMandateId,
                Amount = debitAmount,
                Reference = debitReference,
                Narration = $"LightCap round-up: {data.Narration}"
            }, cancellationToken);

            // 8. Record the attempt in the ledger regardless of outcome.
            var walletTransaction = new WalletTransaction
            {
                Id = Guid.NewGuid(),
                WalletId = wallet.Id,
                UserId = user.Id,
                Type = WalletTransactionType.SpendRoundUp,
                Status = debitResult.Success ? WalletTransactionStatus.Pending : WalletTransactionStatus.Failed,
                Amount = debitAmount,
                AvailableBalanceAfter = wallet.AvailableBalance, // updated only on confirmation webhook
                SourceReference = data.Id,
                MonoDebitReference = debitReference,
                Description = debitResult.Success
                    ? $"Round-up debit initiated (calculated: \u20a6{calculatedRoundUp}, debited: \u20a6{debitAmount})"
                    : $"Debit failed: {debitResult.ErrorMessage}",
                CreatedAt = DateTime.UtcNow
            };

            await walletTransactionRepository.AddAsync(walletTransaction, cancellationToken);
            await walletRepository.SaveChanges(cancellationToken);

            if (!debitResult.Success)
            {
                return Result.Fail(debitResult.ErrorMessage ?? "Debit failed.");
            }

            return Result.Ok(new ProcessMonoTransactionResponse
            {
                RoundUpApplied = true,
                Message = $"Debit of \u20a6{debitAmount} initiated - awaiting confirmation."
            });
        }
    }
}