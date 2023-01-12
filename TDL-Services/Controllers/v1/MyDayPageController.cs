using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    [Route("api/v{version:apiVersion}/myday-page")]
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

        [AllowAnonymous]
        [HttpGet("{id}/todo")]
        public IActionResult GetById(Guid id)
        {
            var response = _todoService.GetTodoById(id);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPut("{id}/todo")]
        public IActionResult UpdateTodo([FromBody] UpdateTodoRequestDto request)
        {
            _todoService.UpdateTodo(request);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("todos")]
        public IActionResult GetAllToDo()
        {
            var response = _todoService.GetAllTodo();

            return Ok(response);
        }
    }
}
