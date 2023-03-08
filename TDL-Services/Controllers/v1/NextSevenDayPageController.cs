using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TDL.Infrastructure.Constants;
using TDL.Services.Dto.NextSevenDayPage;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [EnableCors(ConfigurationConstant.CorsPolicy)]
    [Route("api/v{version:apiVersion}/next-seven-day-page")]
    public class NextSevenDayPageController : BaseController
    {
        private readonly INextSevenDayPageService _nextSevenDayPageService;

        public NextSevenDayPageController(INextSevenDayPageService nextSevenDayPageService)
        {
            _nextSevenDayPageService = nextSevenDayPageService;
        }

        [HttpGet("next-seven-day")]
        public IActionResult GetAllNextSevenDay([FromQuery] GetTodoNextSevenDayRequestDto requestDto)
        {
            var result = _nextSevenDayPageService
                .GetNextSevenDay(request: requestDto,userName: UserName);
            
            return Ok(result);
        }
    }
}
