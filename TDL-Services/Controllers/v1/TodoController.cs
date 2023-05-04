//using Microsoft.AspNetCore.Mvc;
//using System;
//using TDL.Services.Dto.TodoDto;
//using TDL.Services.Services.v1.Interfaces;

//namespace TDL.APIs.Controllers.v1
//{
//    [ApiVersion("1.0")]
//    [ApiExplorerSettings(GroupName = "v1")]
//    [Route("api/v{version:apiVersion}/todo-page")]
//    public class TodoController : BaseController
//    {
//        private readonly ITodoService _todoService;

//        public TodoController(ITodoService todoService)
//        {
//            _todoService = todoService;
//        }

//        [HttpPost("/archive-todo")]
//        public IActionResult ArchieTodo([FromBody] ArchiveTodoRequestDto request)
//        {
//            _todoService.ArchieTodo(request.Id);

//            return Ok();
//        }

//        [HttpPut("/{id}/completed-todo")]
//        public IActionResult CompletedTodo(Guid id)
//        {
//            _todoService.CompletedTodo(id);

//            return Ok();
//        }

//        [HttpGet("not-completed-task-count")]
//        public IActionResult CountTaskNotCompleted([FromQuery] DateTime dateTime)
//        {
//            var response = _todoService.CountTaskNotCompleted(dateTime: dateTime, userName: UserName);

//            object result = new
//            {
//                count = response
//            };

//            return Ok(result);
//        }
//    }
//}