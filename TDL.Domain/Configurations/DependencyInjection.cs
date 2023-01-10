using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetCore.AutoRegisterDi;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TDL.Domain.Context;
using TDL.Infrastructure.Configurations;
using TDL.Infrastructure.Constants;
using TDL.Infrastructure.Persistence.Configurations;
using TDL.Infrastructure.Persistence.Context;
using TypeInfo = System.Reflection.TypeInfo;

namespace TDL.Domain.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureDomainDi(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureDatabase(services, configuration);

            services.RegisterAssemblyPublicNonGenericClasses()
                .Where(c => c.Name.EndsWith("Repository"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            return services;
        }

        private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            var temp = configuration.GetValue<string>(ConfigurationConstant.TdlSchemaName);

            services.AddDbContext<TdlContext>(opts =>
                opts.UseSqlServer(configuration.GetValue<string>(ConfigurationConstant.DefaultConnection),
                    contextBuilder => contextBuilder.MigrationsHistoryTable("__EFMigrationsHistory",
                        configuration.GetValue<string>(ConfigurationConstant.TdlSchemaName))));

            services.AddSingleton<IContextFactory<BaseDbContext>, ContextFactory>(sp =>
                new ContextFactory(new DbContextOptionsBuilder<TdlContext>()
                    .UseSqlServer(configuration.GetValue<string>(ConfigurationConstant.DefaultConnection)).Options,
                    sp.GetService<IHttpContextAccessor>(), sp.GetService<IEnumerable<IEntityTypeConfigurationDependency>>(),
            sp.GetService<IOptions<AppSettings>>()));

            foreach (TypeInfo type in typeof(TdlContext).Assembly.DefinedTypes
                .Where(t => !t.IsAbstract &&
                            !t.IsGenericTypeDefinition &&
                            (typeof(IEntityTypeConfigurationDependency).IsAssignableFrom(t))))
            {
                if (!typeof(IEntityTypeConfigurationDependency).IsAssignableFrom(type))
                {
                    continue;
                }

                services.AddSingleton(typeof(IEntityTypeConfigurationDependency), type);
            }
        }
    }
}
