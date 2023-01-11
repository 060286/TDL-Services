using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TDL.Services.Dto.MyDayPage;
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

        [HttpPost("simple-todo")]
        [AllowAnonymous]
        public IActionResult CreateSimpleTodo([FromBody] CreateSimpleTodoRequestDto request)
        {
            _todoService.CreateSimpleTodo(request);

            return Ok();
        }

        [HttpGet("{id}/todo")]
        public IActionResult GetById(Guid id)
        {
            return Ok();
        }

        [HttpPut("{id}/todo")]
        public IActionResult UpdateTodo([FromBody] UpdateTodoRequestDto request)
        {
            return Ok();
        }
    }
}
