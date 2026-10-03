using CustOps.Infrastructure.Persistence.Repositories;
using LightCap.InvestmentApi.Application.Common.Interfaces;
using LightCap.InvestmentApi.Domain.Entities;
using LightCap.InvestmentApi.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightCap.InvestmentApi.Persistence.Extention.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connStr = configuration.GetConnectionString("LightCap");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connStr, sqlOpt =>
                    sqlOpt.EnableRetryOnFailure())
            );


            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IRepository<UserLogin>, Repository<UserLogin>>();
            services.AddScoped<IRepository<Otp>, Repository<Otp>>();
            

            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
