using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TDL.Services.Dto.MyDayPage;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/myday-page")]
    public class MyDayPageController : BaseController
    {
        private readonly ITodoService _todoService;

        public MyDayPageController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost("simple-todo")]
        public IActionResult CreateSimpleTodo([FromBody] CreateSimpleTodoRequestDto request)
        {
            var response = _todoService.CreateSimpleTodo(request);

            return Ok(response);
        }

        [HttpGet("{id}/todo")]
        public IActionResult GetById(Guid id)
        {
            var response = _todoService.GetTodoById(id);

            Guid userId = UserId;
            string userName = UserName;

            return Ok(response);
        }

        [HttpPut("{id}/todo")]
        public IActionResult UpdateTodo([FromBody] UpdateTodoRequestDto request)
        {
            _todoService.UpdateTodo(request);

            return Ok();
        }

        [HttpGet("todos")]
        public IActionResult GetAllToDo()
        {
            var response = _todoService.GetAllTodo();

            return Ok(response);
        }

        [HttpGet("todo-of-date")]
        public IActionResult GetListTodoOfList([FromQuery] DateTime dateTime)
        {
            IList<TodoOfDateResponseDto> response = _todoService.GetTodoOfDate(dateTime);

            return Ok(response);
        }

        [HttpGet("todo-suggestions-list")]
        public IActionResult GetSuggestTodoList(string keyword)
        {
            var response = _todoService.GetSuggestTodoList(keyword);

            return Ok(response);
        }

        [HttpPut("{id}/completed-task")]
        public IActionResult UpdateCompletedTask(Guid id)
        {
            _todoService.UpdateCompletedOfTask(id);

            return Ok();
        }
    }
}
