using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TDL.Infrastructure.JsonConverters;

namespace TDL.APIs.Configurations
{
    public static class JsonOptions
    {
        public static IMvcBuilder ConfigureJsonOptions(this IMvcBuilder builder)
        {
            var sp = builder.Services.BuildServiceProvider();

            var httpContextAccessor = sp.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;

            builder.AddNewtonsoftJson(cfg =>
            {
                cfg.SerializerSettings.Converters.Add(new DateTimeWithTimezoneConverter(httpContextAccessor));
                cfg.SerializerSettings.Converters.Add(new NullableDateTimeWithTimeZoneConverter(httpContextAccessor));
                cfg.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                cfg.SerializerSettings.Converters.Add(new StringConverter());
            });

            return builder;
        }
    }
}
