using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TDL.Services.Dto.Category;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    // [EnableCors(ConfigurationConstant.CorsPolicy)]
    [Route("api/v{version:apiVersion}/all-list-page")]
    public class MyListsController : BaseController
    {
        private readonly ICategoryService _categoryService;
        
        public MyListsController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Create Simple Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("create-todo-category")]
        public IActionResult CreateCategory([FromBody] CreateCategoryItemRequestDto request)
        {
            _categoryService.CreateCategoryItem(request);

            return Ok();
        }

        /// <summary>
        /// Get Default Category Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("default-category-id")]
        public IActionResult GetDefaultCategoryId()
        {
            var response = _categoryService.GetDefaultCategoryId(UserName);
            
            return Ok(response);
        }
    
        /// <summary>
        /// Get Todos by category name
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpGet("task-by-category")]
        public IActionResult GetTaskByCategory([FromQuery] MyListTodoItemRequestDto requestDto)
        {
            var result = _categoryService.GetMyListTodosItem(requestDto, UserName);
            
            return Ok(result);
        }

        [HttpGet("categories-task")]
        public IActionResult GetAllTaskByUserId()
        {
            var result = _categoryService.GetCategoryByUserName(UserName);
            
            return Ok(result);
        }

        [HttpPost("create-subtask")]
        public IActionResult CreateSubtask([FromBody] CreateSubtaskRequestDto requestDto)
        {
            _categoryService.CreateSubtask(request: requestDto);
            
            return Ok();
        }
    }
}