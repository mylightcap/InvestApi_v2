using LightCap.InvestmentApi.Api.Middleware;

namespace LightCap.InvestmentApi.Api.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddCustomCors()
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails()
            .AddJwtAuthenticationAndAuthorization(configuration)
            .AddHealthAndMvc();
         

		return services;
    }
}