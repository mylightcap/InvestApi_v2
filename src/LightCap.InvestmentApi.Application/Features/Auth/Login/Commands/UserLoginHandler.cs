using FluentResults;
using LightCap.InvestmentApi.Application.Common.Interfaces;
using LightCap.InvestmentApi.Domain.Entities;
using MediatR;

namespace LightCap.InvestmentApi.Application.Features.Auth.Login.Commands
{
    public record UserLoginCommand(LoginDto loginDto) : IRequest<Result<UserLoginResponse>>;

    public class UserLoginHandler(
        IJwtService jwtService,
        IRepository<User> userRepository,
        IRepository<UserLogin> loginRepository
    ) : IRequestHandler<UserLoginCommand, Result<UserLoginResponse>>
    {
        public async Task<Result<UserLoginResponse>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var dto = request.loginDto;

            var user = await userRepository.GetSingleAsync(u => u.Email == dto.Email);

            if (user == null)
                return Result.Fail("Invalid email or password.");

            var passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!passwordValid)
                return Result.Fail("Invalid email or password.");

            // FIX: block login until the user has verified their email via OTP.
            if (!user.IsEmailVerified)
                return Result.Fail("Please verify your email before logging in.");

            // NEW: single-device enforcement.
            // If there's already an active session for this user, refuse the new login
            // until they explicitly log out of the existing one.
            var activeSession = await loginRepository.GetSingleAsync(
                l => l.UserId == user.Id && l.IsActive == true);

            if (activeSession != null)
            {
                return Result.Fail(
                    "You are already logged in on another device. Please log out first.");
            }

            var accessToken = await jwtService.GenerateAccessTokenAsync(user);
            var refreshToken = jwtService.GenerateRefreshToken();

            // FIX: never store the raw refresh token - hash it the same way passwords/OTPs are hashed.
            // If the DB ever leaks, a plaintext refresh token would let an attacker
            // impersonate the user indefinitely (until expiry) with no extra step needed.

            var hashedRefreshToken = BCrypt.Net.BCrypt.HashPassword(refreshToken);

            var login = new UserLogin
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Email = user.Email,

                LoginTime = DateTime.UtcNow,
                IsActive = true,

                IpAddress = dto.IpAddress,
                DeviceId = dto.DeviceId,
                DeviceName = dto.DeviceName,

                RefreshToken = hashedRefreshToken, 
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
            };

            await loginRepository.AddAsync(login, cancellationToken);
            await loginRepository.SaveChanges(cancellationToken);

            return Result.Ok(new UserLoginResponse
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken, // the RAW token goes to the client only the hash is stored
                RefreshTokenExpiry = login.RefreshTokenExpiryTime!.Value
            });
        }
    }
}
