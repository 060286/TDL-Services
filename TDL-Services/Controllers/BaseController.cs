using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TDL.Infrastructure.Extensions;

namespace TDL.APIs.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]s")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected Guid UserId => HttpContext.GetUserId();
        protected string UserName => HttpContext.GetUserName();
        //protected string UserRole => HttpContext.GetUserRole();

        //private static string userClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        //private static string userIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        //private static string userEmailClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";


        //protected Guid UserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(userClaimType)).Value;
        //protected string UserName = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(userIdClaimType)).Value;
        //protected string UserRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == userEmailClaimType).Value;
    }
}
