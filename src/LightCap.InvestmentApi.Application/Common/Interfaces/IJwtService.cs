using LightCap.InvestmentApi.Domain.Entities;
using System.Security.Claims;

namespace LightCap.InvestmentApi.Application.Common.Interfaces;

public interface IJwtService
{
    Task<string> GenerateAccessTokenAsync(User user);

    string GenerateRefreshToken();
}
