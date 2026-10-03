using LightCap.InvestmentApi.Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace LightCap.InvestmentApi.Infrastructure.Extensions.DependencyInjection;

public static class AddServiceOptionsExtensions
{
    public static IServiceCollection AddServiceOptions(this IServiceCollection services)
    {
        services.AddOptions<JwtSettings>()
            .BindConfiguration(JwtSettings.Path)
            .ValidateOnStart();

        services.AddOptions<ServiceBusSettings>()
            .BindConfiguration(ServiceBusSettings.Path)
            .ValidateOnStart();

        services.AddOptions<AzureBlobStorageSettings>()
            .BindConfiguration(AzureBlobStorageSettings.Path)
            .ValidateOnStart();     

     
        return services;
    }
}