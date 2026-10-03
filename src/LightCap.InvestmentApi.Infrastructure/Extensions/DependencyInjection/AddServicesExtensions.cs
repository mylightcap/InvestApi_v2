using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LightCap.InvestmentApi.Infrastructure.Extensions.DependencyInjection;

public static class AddServicesExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
       var connectionString = configuration.GetConnectionString("AzureBlobStorageConnection")!;
        var blobContainerName = configuration.GetValue<string>("AzureBlobStorage:BackOfficeMediaBlobContainerName")!;
        return services
            .AddHttpContextAccessor();
       


        #region Repositories

            //.AddScoped<IUnitOfWork, UnitOfWork>()
            //.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        #endregion

    }
}