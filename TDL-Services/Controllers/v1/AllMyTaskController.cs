using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TDL.Infrastructure.Constants;
using TDL.Services.Dto.MyDayPage;

namespace TDL.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [EnableCors(ConfigurationConstant.CorsPolicy)]
    [Route("api/v{version:apiVersion}/allmytask-page")]
    public class AllMyTaskController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Test()
        {

            return Ok(new AllMyDay
            {
                Title = string.Empty
            });
        }
    }

    public class AllMyDay
    {
        public string Title { get; set; }
    }
}
