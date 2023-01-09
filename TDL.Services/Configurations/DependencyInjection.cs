using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using System;
using System.Collections.Generic;
using System.Text;
using TDL.Services.SignalR;

namespace TDL.Services.Configurations
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Configure the dependency injection for services.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureServiceDi(this IServiceCollection services)
        {
            services.RegisterAssemblyPublicNonGenericClasses()
                .Where(c => c.Name.EndsWith("Services"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            return services;
        }
    }
}
