//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using System.Data;

//namespace CustOps.Infrastructure.Extensions.DependencyInjection;

//public static class AddDbContextExtensions
//{
//    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration config)
//    {
//        services.AddScoped<AuditableEntityInterceptor>();

//        services.AddDbContext<AppDbContext>((sp, options) =>
//            options.UseSqlServer(
//                    config.GetConnectionString("DefaultConnection"),
//                    sqlOptions => sqlOptions.EnableRetryOnFailure(
//                        maxRetryCount: 5,
//                        maxRetryDelay: TimeSpan.FromSeconds(10),
//                        errorNumbersToAdd: null
//                    ))
//                .AddInterceptors(sp.GetServices<SaveChangesInterceptor>()));

//        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());


//        services.AddDbContextFactory<BillsDbContext>(opts =>
//            opts.UseSqlServer(
//                config.GetConnectionString("BillsConnection"),
//                sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

//        services.AddScoped<IBillsDbContext>(sp =>
//            sp.GetRequiredService<IDbContextFactory<BillsDbContext>>().CreateDbContext());

//        services.AddScoped<IBillsDbContextFactory, BillsDbContextFactoryAdapter>();

//        services.AddDbContextFactory<TransferDbContext>(opts =>
//            opts.UseSqlServer(
//                config.GetConnectionString("TransferConnection"),
//                sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

//        services.AddScoped<ITransferDbContext>(sp =>
//            sp.GetRequiredService<IDbContextFactory<TransferDbContext>>().CreateDbContext());

//        services.AddScoped<ITransferDbContextFactory, TransferDbContextFactoryAdapter>();



//        services.AddDbContextFactory<LifestyleDbContext>(opts =>
//            opts.UseSqlServer(
//                config.GetConnectionString("LifestyleConnection"),
//                sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

//        services.AddScoped<ILifestyleDbContext>(sp =>
//            sp.GetRequiredService<IDbContextFactory<LifestyleDbContext>>().CreateDbContext());



//        services.AddDbContext<AirtimeDataDbContext>(sp =>
//    sp.UseSqlServer(
//        config.GetConnectionString("AirtimeAndDataConnection"),
//        sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));
//        services.AddScoped<IAirtimeDataDbContext>(sp => sp.GetRequiredService<AirtimeDataDbContext>());

//        //factory context
//        services.AddDbContextFactory<AppDbContext>(options =>
//                options.UseSqlServer(
//                    config.GetConnectionString("DefaultConnection"),
//                    sqlOptions => sqlOptions.EnableRetryOnFailure(
//                        maxRetryCount: 5,
//                        maxRetryDelay: TimeSpan.FromSeconds(10),
//                        errorNumbersToAdd: null
//                    )),
//            ServiceLifetime.Scoped);

//        services.AddScoped<IAppDbContextFactory, AppDbContextFactoryAdapter>();

//        services.AddDbContext<SynapseContext>(options =>
//            options.UseSqlServer(
//                config.GetConnectionString("SynapseConnection"),
//                sqlOptions => sqlOptions.EnableRetryOnFailure(
//                    maxRetryCount: 5,
//                    maxRetryDelay: TimeSpan.FromSeconds(10),
//                    errorNumbersToAdd: null
//                )));

//        services.AddScoped<Func<string, IDbConnection>>(_ => key =>
//        {
//            return key switch
//            {
//                "Default" => new SqlConnection(config.GetConnectionString("DefaultConnection")),
//                "Transfer" => new SqlConnection(config.GetConnectionString("TransferConnection")),
//                "Bills" => new SqlConnection(config.GetConnectionString("BillsConnection")),
//                "AirtimeAndData" => new SqlConnection(config.GetConnectionString("AirtimeAndDataConnection")),
//                "Lifestyle" => new SqlConnection(config.GetConnectionString("LifestyleConnection")),
//                "Synapse" => new SqlConnection(config.GetConnectionString("SynapseConnection")),
//                "Finacle" => new OracleConnection(config.GetConnectionString("FinacleConnection")),
//                _ => throw new ArgumentException("Invalid connection key")
//            };
//        });

//        return services;
//    }
//}