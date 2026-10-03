using FluentResults;
using LightCap.InvestmentApi.Application.Common.Interfaces;
using LightCap.InvestmentApi.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Application.Features.MonoService.MonoHandler.Command
{
    
        // UserId comes from the authenticated user's JWT (set by the controller),
        // NOT from the request body - same principle as Logout: never trust a
        // client-supplied user identity for anything that writes to their account.
        public record MonoExchangeTokenCommand(Guid UserId, string Code) : IRequest<Result<MonoExchangeTokenResponse>>;


        public class MonoExchangeTokenHandler(IMonoService monoService, IRepository<LinkedBankAccount> linkedAccountRepository
        ) : IRequestHandler<MonoExchangeTokenCommand, Result<MonoExchangeTokenResponse>>
        {
            public async Task<Result<MonoExchangeTokenResponse>> Handle(MonoExchangeTokenCommand request, CancellationToken cancellationToken)
            {
                // Call Mono to swap the temporary code for a permanent Account ID.
                var exchangeResult = await monoService.ExchangeTokenAsync(request.Code, cancellationToken);

                if (!exchangeResult.Success)
                {
                    return Result.Fail(exchangeResult.ErrorMessage ?? "Failed to link bank account.");
                }

                // Avoid saving a duplicate row if the user somehow calls this twice
                // with a code that resolves to an account already linked.
                var alreadyLinked = linkedAccountRepository.Exists(
                    x => x.UserId == request.UserId && x.MonoAccountId == exchangeResult.AccountId);

                if (alreadyLinked)
                {
                    return Result.Ok(new MonoExchangeTokenResponse
                    {
                        AccountId = exchangeResult.AccountId!,
                        Message = "Bank account is already linked."
                    });
                }

                var linkedAccount = new LinkedBankAccount
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    MonoAccountId = exchangeResult.AccountId!,
                    LinkedAt = DateTime.UtcNow,
                    IsActive = true
                };

                await linkedAccountRepository.AddAsync(linkedAccount, cancellationToken);
                await linkedAccountRepository.SaveChanges(cancellationToken);

                return Result.Ok(new MonoExchangeTokenResponse
                {
                    AccountId = exchangeResult.AccountId!,
                    Message = "Bank account linked successfully."
                });
            }
        }
    
}
