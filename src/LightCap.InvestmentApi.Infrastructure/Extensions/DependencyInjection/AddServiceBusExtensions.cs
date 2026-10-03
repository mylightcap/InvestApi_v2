using LightCap.InvestmentApi.Application.Common.Contracts.Abstractions;
using LightCap.InvestmentApi.Infrastructure.Configurations;
using LightCap.InvestmentApi.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LightCap.InvestmentApi.Infrastructure.Extensions.DependencyInjection;

public static class AddServiceBusExtensions
{
    //public static IServiceCollection AddServiceBusConfigurations(this IServiceCollection services,
    //    IConfiguration config)
    //{
    //    var settings = config.GetSection(ServiceBusSettings.Path).Get<ServiceBusSettings>()!;
         

    //    services.AddTransient<IAzureService, AzureService>();

    //    return services;
    //}
}