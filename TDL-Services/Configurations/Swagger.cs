using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using TDL.Infrastructure.Constants;

namespace TDL.APIs.Configurations
{
    public static class Swagger
    {
        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                ServiceProvider serviceProvider = services.BuildServiceProvider();
                IApiVersionDescriptionProvider provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = $"TDL Service Api {description.GroupName}",
                        Version = description.ApiVersion.ToString()
                    });

                    c.DocInclusionPredicate((docName, apiDesc) => apiDesc.GroupName == docName);

                    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                    {
                        Description = ConfigurationConstant.SwaggerAuthorizationDescription,
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    }); 

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                },
                                Scheme = JwtBearerDefaults.AuthenticationScheme,
                                Name = JwtBearerDefaults.AuthenticationScheme,
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });

                    c.OperationFilter<TimeZoneFilter>();
                }
            });
        }   
    }

    public class TimeZoneFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = ConfigurationConstant.TimeZoneKey,
                In = ParameterLocation.Header,
                Description = "Add a optional timezone to specify the locale datetime of returned data. " +
                              "You could find a suitable timezone id to input in this link: 'https://nodatime.org/TimeZones'.",
                Required = true,
                Example = new OpenApiString("Asia/Bangkok")
            });
        }
    }
}
