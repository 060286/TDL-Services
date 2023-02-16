using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDL.Services.Dto.User;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/user")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
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

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequestDto request)
        {
            UserLoginResponseDto response = _userService.LoginAndGetUserToken(request);

            return Ok(response);
        }

        [HttpGet("user-info")]
        public IActionResult GetUserInfo()
        {
            var response = _userService.GetUserInfo(id: UserId);

            return Ok(response);
        }
    }
}
