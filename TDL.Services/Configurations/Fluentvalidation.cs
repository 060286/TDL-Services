using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TDL.Services.Configurations
{
    /// <summary>
    /// Configure the fluent validation
    /// </summary>
    public static class Fluentvalidation
    {
        public static void ConfigureFluentValidation(this IServiceCollection services)
        {
            services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
