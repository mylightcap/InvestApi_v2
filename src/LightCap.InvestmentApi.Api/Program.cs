using LightCap.InvestmentApi.Api.Middleware;
using LightCap.InvestmentApi.Infrastructure.Extensions.DependencyInjection;
using LightCap.InvestmentApi.Persistence.Extention.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpContextAccessor();

        // =========================
        // CONFIGURATION (CLEAN ONLY)
        // =========================
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var env = builder.Environment;

        Console.WriteLine($"Environment detected: {env.EnvironmentName}");

        if (env.IsProduction() || env.IsStaging() ||
            env.EnvironmentName.Equals("Pilot", StringComparison.InvariantCultureIgnoreCase))
        {
            var secretsPath = "/app/Secrets/appsettings.json";

            if (File.Exists(secretsPath))
            {
                Console.WriteLine("Loading production secrets...");
                builder.Configuration.AddJsonFile(secretsPath, optional: true, reloadOnChange: true);
            }
        }

        // =========================
        // DEPENDENCY INJECTION
        // =========================
        builder.Services
            .AddApi(builder.Configuration)
            .AddApplication(builder.Configuration)
            .AddInfrastructure(builder.Configuration)
            .AddPersistence(builder.Configuration);

        // =========================
        // JWT AUTH
        // =========================
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        // =========================
        // MIDDLEWARE PIPELINE (FIXED ORDER)
        // =========================

        // 1. YOUR CUSTOM GLOBAL ERROR HANDLER (IMPORTANT)
        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseRouting();

        app.UseCors("CorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "LightCap Investment API v1");
            c.RoutePrefix = "swagger";
        });

        app.MapControllers();

        app.Run();
    }
}