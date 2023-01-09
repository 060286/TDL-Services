using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TDL.APIs.Configurations
{
    public static class AutoMapper
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.ConstructServicesUsing(serviceProvider.GetService);
                mc.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
            });

            IMapper mapper = mappingConfig.CreateMapper(type => serviceProvider.GetService(type));
            services.AddScoped(typeof(IMapper), sp => mapper);
        }
    }
}
