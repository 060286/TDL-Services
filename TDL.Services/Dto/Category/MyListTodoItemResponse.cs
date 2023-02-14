using System;
using System.Collections.Generic;
using TDL.Services.Dto.Color;

namespace TDL.Services.Dto.Category
{
    public class MyListTodoItemResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int? CompletedSubtask { get; set; }

        public int? TotalSubtask { get; set; }
        
        public bool IsCompleted { get; set; }

        public IList<ColorDto> Priorities { get; set; }
    }
}