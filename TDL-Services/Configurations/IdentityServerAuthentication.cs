using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TDL.Infrastructure.Constants;
using TDL.Infrastructure.Extensions;

namespace EDH.APIs.Configurations
{
    public static class IdentityServerAuthentication
    {
        /// <summary>
        /// Validate Jwt token by using identity server.
        /// </summary>
        public static void ValidateToken(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.Events = new JwtBearerEvents
            //        {
            //            OnMessageReceived = OnMessageReceived,
            //            OnAuthenticationFailed = OnAuthenticationFailed,
            //            OnTokenValidated = token => OnTokenValidated(token, services)
            //        };
            //        options.Authority = string.Format(configuration[ConfigurationConstant.AzureADAuthority], configuration[ConfigurationConstant.AzureADTenantId]);
            //        options.Audience = configuration[ConfigurationConstant.AzureADAudience];
            //    });

            services.AddAuthentication();
        }

        private static Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            switch (context.Exception)
            {
                case UnauthorizedAccessException _:
                    return Task.CompletedTask;

                case SecurityTokenExpiredException _:
                case SecurityTokenInvalidLifetimeException _:
                    throw new UnauthorizedAccessException(ExceptionConstant.ExpiredAuthorizationToken);

                default:
                    throw new UnauthorizedAccessException(ExceptionConstant.InvalidAuthorizationToken);
            }
        }

        private static Task OnTokenValidated(TokenValidatedContext context, IServiceCollection services)
        {
            var userName = GetUserName(context);
            var serviceProvider = services.BuildServiceProvider();
            //var userService = serviceProvider.GetService<IUserService>();
            //var userDto = userService.GetUserByUserName(userName);

            //if (userDto != null)
            //{
            //    context.HttpContext.Items.Add(CommonConstant.Id, userDto.Id);
            //    context.HttpContext.Items.Add(CommonConstant.UserName, userDto.UserName);
            //    context.HttpContext.Items.Add(CommonConstant.UserRole, userDto.Role);
            //}

            return Task.CompletedTask;
        }

        private static Task OnMessageReceived(MessageReceivedContext context)
        {
            if (context.Request.Query.TryGetValue("access_token", out StringValues token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }

        private static string GetUserName(ResultContext<JwtBearerOptions> context)
        {
            var userClaimsIdentity = context.Principal.Identity as ClaimsIdentity;

            var userName =
                userClaimsIdentity?.FindFirst(JwtClaimTypes.PreferredUserName)?.Value?.GetUserNameFromMailAddress();

            return userName;
        }
    }
}
