using System;
using System.Collections.Generic;

namespace TDL.Services.Dto.TodoDto
{
    public class SearchTodoResponseDto
    {
        public IList<SearchTodoItemResponseDto> Tasks { get; set; }

        public IList<SearchTodoItemResponseDto> SubTasks { get; set; }
    }

    public class SearchTodoItemResponseDto
    {
        public Guid? Id { get; set; }

        public string CategoryName { get; set; }

        public Guid? SubTaskId { get; set; }

        public string SubTaskTitle { get; set; }

        public string Title { get; set; }

        public Guid? CategoryId { get; set; }
    }
}