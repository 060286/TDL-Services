using Microsoft.AspNetCore.Mvc;
using System;
using TDL.Infrastructure.Extensions;

namespace TDL.APIs.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]s")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected Guid UserId => HttpContext.GetUserId();
        protected string UserName => HttpContext.GetUserName();
        protected string UserRole => HttpContext.GetUserRole();
    }
}
