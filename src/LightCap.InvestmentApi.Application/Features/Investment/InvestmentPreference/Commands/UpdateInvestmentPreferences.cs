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

namespace LightCap.InvestmentApi.Application.Features.Investment.InvestmentPreference.Commands
{
    public class UpdateInvestmentPreferences
    {
        // UserId comes from the authenticated user's JWT (set by the controller),
        // never from the request body - same principle used in Logout and Mono exchange.
        public record UpdateInvestmentPreferencesCommand(Guid UserId, decimal AutoInvestPercentage, 
                        InvestmentMode InvestmentMode) : IRequest<Result<UpdateInvestmentPreferencesResponse>>;

        

        public class UpdateInvestmentPreferencesHandler(
                        IRepository<User> userRepository) : 
                        IRequestHandler<UpdateInvestmentPreferencesCommand, Result<UpdateInvestmentPreferencesResponse>>
        {
            // Sensible guardrails - a 0% or negative round-up does nothing useful,
            // and anything above 100% would mean investing more than was spent.
            // Adjust these bounds if your product rules differ (e.g. max 50%).
            private const decimal MinPercentage = 1;
            private const decimal MaxPercentage = 100;

            public async Task<Result<UpdateInvestmentPreferencesResponse>> Handle(
                UpdateInvestmentPreferencesCommand request,
                CancellationToken cancellationToken)
            {
                if (request.AutoInvestPercentage < MinPercentage || request.AutoInvestPercentage > MaxPercentage)
                {
                    return Result.Fail(
                        $"Auto-invest percentage must be between {MinPercentage}% and {MaxPercentage}%.");
                }

                var user = await userRepository.GetByIdAsync(request.UserId);

                if (user == null)
                    return Result.Fail("User not found.");

                user.AutoInvestPercentage = request.AutoInvestPercentage;
                user.InvestmentMode = request.InvestmentMode;

                await userRepository.UpdateAsync(user);
                await userRepository.SaveChanges(cancellationToken);

                return Result.Ok(new UpdateInvestmentPreferencesResponse
                {
                    AutoInvestPercentage = user.AutoInvestPercentage,
                    InvestmentMode = user.InvestmentMode.ToString(),
                    Message = "Investment preferences updated successfully."
                });
            }
        }
    }
}
