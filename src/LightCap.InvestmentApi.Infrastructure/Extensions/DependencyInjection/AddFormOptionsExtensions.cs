using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LightCap.InvestmentApi.Infrastructure.Extensions.DependencyInjection;

public static class AddFormOptionsExtensions
{
    public static IServiceCollection AddFormOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = long.MaxValue;
            options.BufferBody = false; // prevents writing to /tmp
            options.BufferBodyLengthLimit = 0;
        });
        return services;
    }
}