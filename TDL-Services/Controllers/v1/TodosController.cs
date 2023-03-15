using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using TDL.Services.Dto.TodoDto;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/todos")]
    public class TodosController : BaseController
    {
        private readonly ITodoService _todoService;

        public TodosController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPut("{id}/archive-todo")]
        public IActionResult ArchieTodo(Guid id)
        {
            _todoService.ArchieTodo(id);

            return Ok();
        }

        [HttpPut("{id}/completed-todo")]
        public IActionResult CompletedTodo(Guid id)
        {
            var response = _todoService.CompletedTodo(id);

            return Ok(response);
        }

        [HttpGet("not-completed-task-count")]
        public IActionResult CountTaskNotCompleted([FromQuery] DateTime dateTime)
        {
            var response = _todoService.CountTaskNotCompleted(dateTime: dateTime, userName: UserName);

            object result = new
            {
                count = response
            };

            return Ok(result);
        }

        [HttpPut("{id}/update-todo-title")]
        public IActionResult UpdateTitleOfTodo(Guid id, [FromBody] UpdateTitleOfTodoRequest request)
        {
            var response = _todoService.UpdateTodoTitle(id, request.Title);

            return Ok(response);
        }

        [HttpPut("{id}/update-todo-description")]
        public IActionResult UpdateDescriptionOfTodo(Guid id, [FromBody] UpdateDescriptionOfTodoRequest request)
        {
            var response = _todoService.UpdateTodoTitle(id, request.Description);

            return Ok(response);
        }

        [HttpGet("tags-list")]
        [AllowAnonymous]
        public IActionResult GetTagsList()
        {
            var response = _todoService.GetTagList();

            return Ok(response);
        }
    }

    public class UpdateTitleOfTodoRequest
    {
        public string Title { get; set; }
    }

    public class UpdateDescriptionOfTodoRequest
    {
        public string Description { get; set; }
    }
}
