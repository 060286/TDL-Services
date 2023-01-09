using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace TDL.APIs.Configurations
{
    public static class ApiVersioning
    {
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddVersionedApiExplorer(cfg =>
            {
                cfg.GroupNameFormat = "'v'VVV";

                cfg.SubstituteApiVersionInUrl = true;
            });

            services.AddApiVersioning(cfg =>
            {
                // Specify the default version of the API is 1.0
                cfg.DefaultApiVersion = new ApiVersion(1, 0);

                // If the client application doesn't specify the API version, assume to use the default version.
                cfg.AssumeDefaultVersionWhenUnspecified = true;

                // Let the consumer (e.g client application) know about the supported API version.
                cfg.ReportApiVersions = true;

                // Set the error response if the request version does not match
                cfg.ErrorResponses = new ApiVersioningErrorResponseProvider();
            });
        }
    }
}
