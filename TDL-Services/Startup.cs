using TDL.APIs.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TDL.APIs.Extensions;
using TDL.Infrastructure.Constants;
using TDL.Services.Configurations;
using TDL.Services.SignalR.Hubs;

namespace TDL_Services
{
    public class Startup
    {
        public static bool IsDebug
        {
            get
            {
                bool isDebug = false;
                #if DEBUG
                isDebug = true;
                #endif
                return isDebug;
            }
        }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            if (!IsDebug)
            {
                builder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            }

            Configuration = builder.AddEnvironmentVariables().Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDi(Configuration);

            //services.ConfigureHangFire(Configuration, IsDebug);

            services.AddControllers()
                .ConfigureApiBehaviorOptions().ConfigureJsonOptions();

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser().Build();
            });

            services.ConfigureFluentValidation();

            services.ConfigureSwagger(Configuration);

            services.AllowCors(Configuration);

            services.ConfigureApiVersioning();

            services.ConfigureAutoMapper();

            services.ValidateToken(Configuration);

            services.AddSignalR();

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(ConfigurationConstant.CorsPolicy);

            app.UseCustomResponseWrapper();

            app.UseCustomExceptionHandler();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(opts =>
            {
                var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    opts.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/hubs/notification");
                endpoints.MapHealthChecks(ConfigurationConstant.HealthCheckPath, new HealthCheckOptions());
                endpoints.MapControllers().RequireAuthorization();
            });

            loggerFactory.AddFile(Configuration.GetSection(ConfigurationConstant.LoggingSection));

            //app.UseHangfireDashboard();

            //HangFire.RunRecurringJobs(IsDebug);
        }
    }
}
