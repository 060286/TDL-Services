using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using TDL.Infrastructure.Constants;
using TDL.Services.Dto.MyDayPage;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [EnableCors(ConfigurationConstant.CorsPolicy)]
    [Route("api/v{version:apiVersion}/allmytask-page")]
    public class AllMyTaskController : BaseController
    {
        private readonly IAllMyTaskPageService _allMyTaskPageService;

        public AllMyTaskController(IAllMyTaskPageService allMyTaskPageService)
        {
            _allMyTaskPageService = allMyTaskPageService;
        }

        
        /// <summary>
        /// Get All Task In All Task Page
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpGet("all-task")]
        public IActionResult GetAllTask(DateTime dateTime)
        {
            var response = _allMyTaskPageService.GetAllTask(dateTime);

            return Ok(response);
        }
    }
}