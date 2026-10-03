using FluentResults;
using LightCap.InvestmentApi.Application.Common.Interfaces;
using LightCap.InvestmentApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LightCap.InvestmentApi.Application.Features.Auth.Registration.Commands
{
    public record UserRegistrationCommand(UserRegistrationDto UserRegistrationDto) : IRequest<Result<UserRegistrationResponse>>;

    public class UserRegistrationHandler(
        IRepository<User> repository,
        IEmailService emailService,
        ISmsService smsService,
        IOtpService otpService,
        IRepository<Otp> otpRepository,
        IConfiguration config) : IRequestHandler<UserRegistrationCommand, Result<UserRegistrationResponse>>
    {
        public async Task<Result<UserRegistrationResponse>> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UserRegistrationDto;

            var existingUser = repository.Exists(x => x.Email == dto.Email);

            if (existingUser)
            {
                return Result.Fail("User with this email already exists.");
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                return Result.Fail("Password and Confirm Password do not match.");
            }

            var userId = Guid.NewGuid();

            var otpLength = int.Parse(config["OTP:Length"]!);
            var generatedOtp = otpService.GenerateOtp();
            var hashedOtp = BCrypt.Net.BCrypt.HashPassword(generatedOtp);
            var expiryMinutes = int.Parse(config["OTP:ExpiryMinutes"]!);

            var name = 1;

            var emailSubject = "Email Verification OTP";
            var emailContent =
                $"Your OTP for email verification is: {generatedOtp}. " +
                $"It will expire in {expiryMinutes} minutes.";

            var otp = new Otp
            {
                UserId = userId,
                Email = dto.Email,
                Code = hashedOtp,
                ExpiryTime = DateTime.UtcNow.AddMinutes(expiryMinutes),
                IsUsed = false,
                AttemptCount = 0,
                CreatedAt = DateTime.UtcNow
            };

            var user = new User
            {
                Id = userId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MiddleName = dto.MiddleName,
                DateOfBirth = dto.DateOfBirth,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Gender = dto.Gender,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                AcceptTermsAndConditions = dto.AcceptTermsAndConditions,
                AcceptPrivacyPolicy = dto.AcceptPrivacyPolicy,
                DeviceId = dto.DeviceId,
                DeviceName = dto.DeviceName,
                DeviceType = dto.DeviceType,
                IpAddress = dto.IpAddress,
                Country = dto.Country,
                State = dto.State,
                City = dto.City
            };

            // FIX: SqlServerRetryingExecutionStrategy does not allow a manually
            // started transaction on its own - wrap it in an execution strategy
            // so EF Core can safely retry the whole block as one unit.
            var strategy = repository.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await repository.BeginTransactionAsync(cancellationToken);
                try
                {
                    await repository.AddAsync(user, cancellationToken);
                    await otpRepository.AddAsync(otp, cancellationToken);

                    await repository.SaveChanges(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });

            // FIX: only send the OTP email AFTER the transaction above has
            // committed successfully. If this ran any earlier (before or
            // during the transaction), a user could receive a valid-looking
            // OTP email for an account that never actually got saved -
            // exactly what happened before this fix.
            await emailService.SendEmailWithFallback(dto.Email, emailSubject, emailContent);
           // await smsService.SendSmsAsync(dto.PhoneNumber, emailContent, cancellationToken);

            return Result.Ok(new UserRegistrationResponse
            {
                UserId = userId,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsEmailVerified = false,
                IsPhoneVerified = false,
                RequiresOtpVerification = true,
                CreatedAt = DateTime.UtcNow,
                Message = "User registered successfully. Please verify your email and phone number."
            });
        }
    }
}