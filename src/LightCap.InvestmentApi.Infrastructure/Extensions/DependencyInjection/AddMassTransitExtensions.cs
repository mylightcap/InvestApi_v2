using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LightCap.InvestmentApi.Infrastructure.Extensions.DependencyInjection;

public static class AddMassTransitExtensions
{
    public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddMassTransit(x =>
        //{           
        //});
        return services;
    }
}