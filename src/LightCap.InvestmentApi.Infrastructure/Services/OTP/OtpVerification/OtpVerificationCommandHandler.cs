using BCrypt.Net;
using FluentResults;
using LightCap.InvestmentApi.Application.Common.Interfaces;
using LightCap.InvestmentApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LightCap.InvestmentApi.Infrastructure.Services.OTP.OtpVerification;

public record OtpVerificationCommand(string Email, string OtpCode)
    : IRequest<Result<OtpVerificationCommandOutput>>;

public class OtpVerificationCommandHandler(IRepository<Otp> otpRepository, IRepository<User> userRepository)
    : IRequestHandler<OtpVerificationCommand, Result<OtpVerificationCommandOutput>>
{
    public async Task<Result<OtpVerificationCommandOutput>> Handle(
        OtpVerificationCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            //  GET LATEST OTP FOR EMAIL
            var otp = await otpRepository.GetQueryable()
                .Where(x => x.Email == request.Email && !x.IsUsed)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (otp == null)
            {
                return Result.Fail<OtpVerificationCommandOutput>(
                    "No OTP request found for this email.");
            }

            
            if (otp.ExpiryTime < DateTime.UtcNow)
            {
                return Result.Fail<OtpVerificationCommandOutput>(
                    "OTP has expired.");
            }


            if (otp.AttemptCount >= 5)
                return Result.Fail<OtpVerificationCommandOutput>("Too many incorrect attempts. Please request a new code.");

            var isValidOtp = BCrypt.Net.BCrypt.Verify(request.OtpCode, otp.Code);

            if (!isValidOtp)
            {
                otp.AttemptCount++;
                await otpRepository.UpdateAsync(otp);
                await otpRepository.SaveChanges(cancellationToken);
                return Result.Fail<OtpVerificationCommandOutput>("Invalid OTP.");
            }


            otp.IsUsed = true;

            var user = await userRepository.GetSingleAsync(u => u.Email == request.Email);
            if (user == null)
                return Result.Fail<OtpVerificationCommandOutput>("No account found for this email.");

            user.IsEmailVerified = true;

            await otpRepository.UpdateAsync(otp);
            await userRepository.UpdateAsync(user);
            await otpRepository.SaveChanges(cancellationToken);

            
            return Result.Ok(new OtpVerificationCommandOutput
            {
                IsVerified = true,
                Email = request.Email,
                Message = "OTP verified successfully."
            });
        }
        catch (Exception ex)
        {
            return Result.Fail<OtpVerificationCommandOutput>(
                $"OTP verification failed: {ex.Message}");
        }
    }
}