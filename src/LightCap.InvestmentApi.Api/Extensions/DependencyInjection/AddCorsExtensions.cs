namespace LightCap.InvestmentApi.Api.Extensions.DependencyInjection;

public static class AddCorsExtensions
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
                policy.WithOrigins("http://localhost:3006")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
        });

        return services;
    }
}