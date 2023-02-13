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
        //[HttpGet("all-task")]
        //[AllowAnonymous]
        //public IActionResult GetAllTask(DateTime dateTime)
        //{
        //    var response = _allMyTaskPageService.GetAllTask(dateTime);

        //    return Ok(response);
        //}
    }
    
}
