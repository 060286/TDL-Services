using System;
using Microsoft.AspNetCore.Mvc;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/myday-page")]
    public class TodoController : BaseController
    {
        private readonly ITodoService _todoService;
        
        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("/{id}/archie-todo")]
        public IActionResult ArchieTodo(Guid id)
        {
            _todoService.ArchieTodo(id);
            
            return Ok();
        }

        [HttpGet("/{id}/completed-todo")]
        public IActionResult CompletedTodo(Guid id)
        {
            _todoService.CompletedTodo(id);
            
            return Ok();
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
    }
}