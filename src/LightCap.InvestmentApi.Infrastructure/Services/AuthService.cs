using LightCap.InvestmentApi.Application.Common.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LightCap.InvestmentApi.Infrastructure.Services;

public class AuthService(
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    private readonly ClaimsPrincipal _user = httpContextAccessor.HttpContext?.User ??
                                             throw new ArgumentNullException(nameof(httpContextAccessor));

    
}