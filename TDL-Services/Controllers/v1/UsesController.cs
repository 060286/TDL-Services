using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using TDL.Infrastructure.Extensions;
using TDL.Services.Dto.User;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/user")]
    public class UsesController : BaseController
    {
        private readonly IUserService _userService;

        public UsesController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult RegisterAccount([FromBody] RegisterAccountRequestDto request)
        {
            _userService.RegisterAccount(request);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("search-user")]
        public IActionResult SearchUser([FromQuery] string keyword)
        {
            var response = _userService.SearchUserInfo(keyword);

            return Ok(response);
        }
    }
}
