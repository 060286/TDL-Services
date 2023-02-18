using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using TDL.Services.Services.v1;
using TDL.Services.Services.v1.Interfaces;
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
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            // services.AddScoped<ITodoService, TodoService>();

            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            return services;
        }
    }
}
