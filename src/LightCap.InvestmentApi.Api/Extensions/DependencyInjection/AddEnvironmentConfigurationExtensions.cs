namespace LightCap.InvestmentApi.Api.Extensions.DependencyInjection;

public static class AddEnvironmentConfigurationExtensions
{
    public static WebApplicationBuilder BuildEnvironmentConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());

        builder.Configuration.AddJsonFile("secrets/appsettings.json", optional: true, reloadOnChange: true);

        var swaggerFilterUrl = builder.Configuration.GetValue<string>("SwaggerFilterUrl") ?? "";
        if (swaggerFilterUrl.Contains("swagger", StringComparison.OrdinalIgnoreCase))
        {
            builder.Configuration.AddJsonFile("appsetting.json", optional: true, reloadOnChange: true);
        }

        if (builder.Environment.EnvironmentName.StartsWith("dev", StringComparison.OrdinalIgnoreCase))
        {
            builder.Configuration.AddJsonFile("appsetting.development.json", optional: true, reloadOnChange: true);
        }

        builder.Configuration.AddEnvironmentVariables();

        return builder;
    }
}