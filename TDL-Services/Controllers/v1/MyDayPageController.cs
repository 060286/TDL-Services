using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/users")]
    public class MyDayPageController : BaseController
    {
        private readonly ITodoService _todoService;

        public MyDayPageController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [AllowAnonymous]
        [HttpGet("{id}/tesing-my-day")]
        public IActionResult Test(Guid id)
        {
            var result = _todoService.GetTodoById(id);

            return Ok(result);
        }
    }
}
