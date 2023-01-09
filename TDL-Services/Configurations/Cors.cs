﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TDL.Infrastructure.Constants;

namespace TDL.APIs.Configurations
{
    public static class Cors
    {
        public static void AllowCors(this IServiceCollection services, IConfiguration configuration)
        {
            var teamp = configuration.GetSection(ConfigurationConstant.AllowedOrigins).Get<string[]>();
            services.AddCors(options =>
            {
                options.AddPolicy(ConfigurationConstant.CorsPolicy,
                    builders =>
                    {
                        builders
                            //.AllowAnyOrigin()
                            //.WithOrigins(configuration.GetSection(ConfigurationConstant.AllowedOrigins).Get<string[]>())
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
        }
    }
}
