using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TDL.Domain.Configurations;
using TDL.Infrastructure.Configurations;
using TDL.Services.Configurations;

namespace TDL.APIs.Configurations
{
    public static class DependencyInjection
    {
        public static void ConfigureDi(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureInfrastructureDi(configuration)
                .ConfigureDomainDi(configuration)
                .ConfigureServiceDi();

            services.AddTransient<IAuthorizationHandler, CustomAuthorizationHandler>();
        }
    }
}
