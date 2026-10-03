using LightCap.InvestmentApi.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace LightCap.InvestmentApi.Infrastructure.Services.OTP.TokenGenerator;

public  class OtpService : IOtpService
{
    private const string Digits = "0123456789";
    private readonly IConfiguration _configuration;

    public OtpService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateOtp()
    {
        var length = int.Parse(_configuration["OTP:Length"]!);
        var otp = new StringBuilder();
        using var rng = RandomNumberGenerator.Create();

        byte[] buffer = new byte[sizeof(uint)];
        while (otp.Length < length)
        {
            rng.GetBytes(buffer);
            uint num = BitConverter.ToUInt32(buffer, 0);
            otp.Append(Digits[(int)(num % Digits.Length)]);
        }

        return otp.ToString();
    }

   
}
