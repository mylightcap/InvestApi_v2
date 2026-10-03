using FluentResults;
using LightCap.InvestmentApi.Application.Common.Interfaces;
using LightCap.InvestmentApi.Application.Common.Interfaces.Repository;
using LightCap.InvestmentApi.Domain.Entities;
using LightCap.InvestmentApi.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.MonoMandate.Commands
{
    public record SetupMandateCommand(Guid UserId, decimal MaxAmount) : IRequest<Result<SetupMandateResponse>>;

    public class SetupMandateHandler(
        IMonoMandateService monoMandateService,
        IRepository<User> userRepository,
        IRepository<LinkedBankAccount> linkedAccountRepository,
        IRepository<DirectDebitMandate> mandateRepository
    ) : IRequestHandler<SetupMandateCommand, Result<SetupMandateResponse>>
    {
        public async Task<Result<SetupMandateResponse>> Handle(
            SetupMandateCommand request,
            CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId);

            if (user == null)
                return Result.Fail("User not found.");

            // Both prerequisites must already be complete before a mandate can be created.
            if (string.IsNullOrEmpty(user.MonoCustomerId))
                return Result.Fail("Please complete your BVN/customer setup before enabling automatic debits.");

            var linkedAccount = await linkedAccountRepository.GetSingleAsync(
                x => x.UserId == user.Id && x.IsActive);

            if (linkedAccount == null)
                return Result.Fail("Please link a bank account before enabling automatic debits.");

            // Avoid creating a duplicate mandate if one already exists (pending or ready).
            var existingMandate = await mandateRepository.GetSingleAsync(
                x => x.UserId == user.Id
                     && (x.Status == MandateStatus.Pending || x.Status == MandateStatus.ReadyToDebit));

            if (existingMandate != null)
            {
                return Result.Ok(new SetupMandateResponse
                {
                    Status = existingMandate.Status.ToString(),
                    Message = "A mandate already exists for this user."
                });
            }

            var reference = $"mandate-{user.Id}-{Guid.NewGuid():N}";

            var createResult = await monoMandateService.CreateMandateAsync(new MonoCreateMandateRequest
            {
                MonoCustomerId = user.MonoCustomerId,
                AccountId = linkedAccount.MonoAccountId,
                MaxAmount = request.MaxAmount,
                Reference = reference
            }, cancellationToken);

            if (!createResult.Success)
            {
                return Result.Fail(createResult.ErrorMessage ?? "Failed to set up automatic debit mandate.");
            }

            var mandate = new DirectDebitMandate
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                MonoMandateId = createResult.MonoMandateId!,
                Status = MandateStatus.Pending, // NOT debitable yet - waiting on Mono's "ready-to-debit" webhook
                CreatedAt = DateTime.UtcNow
            };

            await mandateRepository.AddAsync(mandate, cancellationToken);
            await mandateRepository.SaveChanges(cancellationToken);

            return Result.Ok(new SetupMandateResponse
            {
                Status = mandate.Status.ToString(),
                Message = "Mandate created. This can take a few minutes to a few hours to become active - " +
                           "we'll notify you once automatic investing is ready."
            });
        }
    }
}
