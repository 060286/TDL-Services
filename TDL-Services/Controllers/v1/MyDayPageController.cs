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

        /// <summary>
        /// My Day Page
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("simple-todo")]
        public IActionResult CreateSimpleTodo([FromBody] CreateSimpleTodoRequestDto request)
        {
            
            var response = _todoService.CreateSimpleTodo(request, UserName);

            return Ok(response);
        }

        /// <summary>
        /// Get Detail Todo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/todo")]
        public IActionResult GetById(Guid id)
        {
            var response = _todoService.GetTodoById(id);

            return Ok(response);
        }

        /// <summary>
        /// Update Todo
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
        public IActionResult GetListTodoByDateTime([FromQuery] DateTime dateTime)
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

        [HttpGet("change-todo-category")]
        public IActionResult ChangeTodoCategory([FromQuery] ChangeTodoCategoryRequestDto request)
        {
            var response = _todoService.ChangeCategoryTitle(request, UserName);

            return Ok(response);
        }

        [HttpGet("change-tag")]
        public IActionResult ChangeTodoTag([FromQuery] ChangeTodoTagRequestDto request)
        {
            return Ok();
        }

        [HttpPut("{id}/subtask-complete-status")]
        public IActionResult UpdateSubTaskCompletedStatus(Guid id)
        {
            _todoService.UpdateSubTaskStatus(id);

            return Ok();
        } 
    }
}
