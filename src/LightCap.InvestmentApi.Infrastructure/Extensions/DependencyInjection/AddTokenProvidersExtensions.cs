using LightCap.InvestmentApi.Infrastructure.Services.Integrations.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace LightCap.InvestmentApi.Infrastructure.Extensions.DependencyInjection;

public static class AddTokenProvidersExtensions
{
    public static IServiceCollection AddTokenProviders(this IServiceCollection services)
    {
        services.AddTransient(typeof(AuthHeaderHandler<>));
        return services;
    }
}