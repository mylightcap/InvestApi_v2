using Azure.Storage.Blobs;
using LightCap.InvestmentApi.Application.Common.Behaviors;
using LightCap.InvestmentApi.Infrastructure.Configurations;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LightCap.InvestmentApi.Application.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
       // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

       // services.AddAutoMapper(_ => { }, Assembly.GetExecutingAssembly());

       // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

       // var connectionString = configuration.GetConnectionString("AzureBlobStorageConnection");

        //services.AddSingleton(_ =>
        //    string.IsNullOrWhiteSpace(connectionString)
        //        ? new BlobServiceClient(new Uri("https://placeholder.blob.core.windows.net"))
        //        : new BlobServiceClient(connectionString)
        //);
        services.AddOptions<AppSettings>();
  //.BindConfiguration(AppSettings.Path)
  //.ValidateOnStart();

        
        return services;
    }
}