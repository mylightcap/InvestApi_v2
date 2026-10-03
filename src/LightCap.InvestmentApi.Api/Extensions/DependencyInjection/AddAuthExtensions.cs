using LightCap.InvestmentApi.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace LightCap.InvestmentApi.Api.Extensions.DependencyInjection;

public static class AddAuthExtensions
{
    public static IServiceCollection AddJwtAuthenticationAndAuthorization(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultScheme =
                    options.DefaultAuthenticateScheme =
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration[JwtSettings.Path + ":Issuer"],

                    ValidateAudience = true,
                    ValidAudience = configuration[JwtSettings.Path + ":Audience"],

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1),

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Convert.FromBase64String(configuration[JwtSettings.Path + ":Key"]!).Take(32).ToArray()),
                };
            });


        services.AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());
           
        return services;
    }
}