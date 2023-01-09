using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using TDL.Infrastructure.Constants;
using TDL.Infrastructure.Extensions;

namespace TDL.APIs.Configurations
{
    public class CustomAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
        {
            var allowedRole = requirement.AllowedRoles.FirstOrDefault();
            var userRole = _httpContextAccessor.HttpContext.GetUserRole();

            bool isAuthorized;
            switch (allowedRole)
            {
                case RoleConstant.Admin:
                    isAuthorized = !string.IsNullOrWhiteSpace(userRole) && userRole.Equals(allowedRole);
                    break;

                case null:
                    isAuthorized = true;
                    break;

                default:
                    isAuthorized = false;
                    break;
            }

            if (isAuthorized) 
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
