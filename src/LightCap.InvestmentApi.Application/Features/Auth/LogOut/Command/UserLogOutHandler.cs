using FluentResults;
using LightCap.InvestmentApi.Application.Common.Interfaces;
using LightCap.InvestmentApi.Application.Features.Auth.LogOut;
using LightCap.InvestmentApi.Domain.Entities;
using MediatR;

namespace LightCap.InvestmentApi.Application.Features.Auth.Logout.Commands
{
    // Pass the UserId from the authenticated user's JWT claims (not from the request body) -
    // never trust a client-supplied UserId for a security-sensitive action like this.
    public record UserLogoutCommand(Guid UserId) : IRequest<Result<UserLogOutResponse>>;

    public class UserLogoutHandler(IRepository<UserLogin> loginRepository) : IRequestHandler<UserLogoutCommand, Result<UserLogOutResponse>>
    {
        public async Task<Result<UserLogOutResponse>> Handle(UserLogoutCommand request, CancellationToken cancellationToken)
        {
            var activeSession = await loginRepository.GetSingleAsync(
               l => l.UserId == request.UserId && l.IsActive == true);

            if (activeSession == null)
                return Result.Fail("No active session found.");

            activeSession.IsActive = false;
            activeSession.LogoutTime = DateTime.UtcNow;

            // Optional but recommended: clear the stored refresh token hash on logout
            // so it can never be used again even if somehow leaked before expiry.
            activeSession.RefreshToken = null;

            await loginRepository.UpdateAsync(activeSession);
            await loginRepository.SaveChanges(cancellationToken);

            return Result.Ok();

        }
    }
}
