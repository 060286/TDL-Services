using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TDL.Infrastructure.Persistence.Context;
using TDL.Infrastructure.Persistence.Repositories.Repositories;
using TDL.Infrastructure.Persistence.UnitOfWork;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;

namespace TDL.Infrastructure.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureInfrastructureDi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IUnitOfWorkProvider, UnitOfWorkProvider>(
                imp => new UnitOfWorkProvider(imp.GetService<IContextFactory<BaseDbContext>>()));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<AppSettings>(opts => configuration.GetSection(nameof(AppSettings)).Bind(opts));

            //services.AddSingleton<IBlobStorageService, BlobStorageService>();

            //services.AddSingleton<IEdhApiService, EdhApiService>();

            //services.AddSingleton<IMSGraphService, MSGraphService>();

            return services;
        }
    }
}
