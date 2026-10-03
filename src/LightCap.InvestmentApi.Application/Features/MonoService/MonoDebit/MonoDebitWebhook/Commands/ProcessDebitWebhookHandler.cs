using FluentResults;
using LightCap.InvestmentApi.Application.Common.Interfaces;
using LightCap.InvestmentApi.Domain.Entities;
using LightCap.InvestmentApi.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.MonoDebit.MonoDebitWebhook.Commands
{
    public record ProcessDebitWebhookCommand(MonoDebitWebhookPayload Payload) : IRequest<Result>;

    public class ProcessDebitWebhookHandler(
        IRepository<WalletTransaction> walletTransactionRepository,
        IRepository<Wallet> walletRepository
    ) : IRequestHandler<ProcessDebitWebhookCommand, Result>
    {
        public async Task<Result> Handle(ProcessDebitWebhookCommand request, CancellationToken cancellationToken)
        {
            var data = request.Payload.Data;

            // Find the exact WalletTransaction this webhook confirms, using the
            // reference we generated and sent to Mono at debit time.
            var transaction = await walletTransactionRepository.GetSingleAsync(
                x => x.MonoDebitReference == data.Reference);

            if (transaction == null)
                return Result.Fail("No matching wallet transaction found for this debit reference.");

            // Idempotency: if this transaction is already Completed or Failed,
            // don't process the same confirmation twice (Mono can retry webhooks).
            if (transaction.Status != WalletTransactionStatus.Pending)
            {
                return Result.Ok();
            }

            var wallet = await walletRepository.GetByIdAsync(transaction.WalletId);

            if (wallet == null)
                return Result.Fail("Wallet not found for this transaction.");

            var isSuccess = request.Payload.Event.Contains("successful", StringComparison.OrdinalIgnoreCase);

            if (isSuccess)
            {
                transaction.Status = WalletTransactionStatus.Completed;
                transaction.CompletedAt = DateTime.UtcNow;

                // THIS is the moment real money is confirmed to have landed.
                wallet.AvailableBalance += transaction.Amount;
                wallet.UpdatedAt = DateTime.UtcNow;

                transaction.AvailableBalanceAfter = wallet.AvailableBalance;
            }
            else
            {
                transaction.Status = WalletTransactionStatus.Failed;
                transaction.Description += " | Debit failed per Mono confirmation webhook.";
            }

            await walletTransactionRepository.UpdateAsync(transaction);
            await walletRepository.UpdateAsync(wallet);
            await walletRepository.SaveChanges(cancellationToken);

            return Result.Ok();
        }
    }
}

    
