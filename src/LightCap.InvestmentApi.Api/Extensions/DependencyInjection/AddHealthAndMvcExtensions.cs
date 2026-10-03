namespace LightCap.InvestmentApi.Api.Extensions.DependencyInjection;

public static class AddHealthAndMvcExtensions
{
    public static IServiceCollection AddHealthAndMvc(this IServiceCollection services)
    {
        services.AddHealthChecks();
        services.AddControllers();
        //services.AddApplicationInsightsTelemetry();
        services.AddEndpointsApiExplorer();

        return services;
    }
}