using System.Security.Cryptography;

namespace LightCap.InvestmentApi.Infrastructure.Services.OTP.TokenGenerator;

public class RefreshTokenGenerator
{   
    public static string GenerateRefreshToken(int size = 64)
    {
        var randomNumber = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
