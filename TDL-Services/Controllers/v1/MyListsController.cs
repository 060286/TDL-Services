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
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpPost]
        public IActionResult CreateCategory([FromBody] CreateCategoryItemRequestDto request)
        {
            _categoryService.CreateCategoryItem(request);

            return Ok();
        }
    
        /// <summary>
        /// Get Todos by category name
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpGet("task-by-category")]
        public IActionResult GetTaskByCategory([FromQuery] MyListTodoItemRequestDto requestDto)
        {
            var result = _categoryService.GetMyListTodosItem(requestDto);
            
            return Ok(result);
        }

        [HttpGet("categories-task")]
        public IActionResult GetAllTaskByUserId()
        {
            var result = _categoryService.GetCategoryByUserName(UserName);
            
            return Ok(result);
        }
    }
}