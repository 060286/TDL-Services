using Microsoft.AspNetCore.Builder;
using TDL.APIs.Configurations;

namespace TDL.APIs.Extensions
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseCustomResponseWrapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomResponseWrapper>();
        }

        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandler>();
        }
    }
}
