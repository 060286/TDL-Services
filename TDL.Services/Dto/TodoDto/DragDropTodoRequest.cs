using System;
using System.Collections.Generic;
using System.Text;

namespace TDL.Services.Dto.TodoDto
{
    public class DragDropTodoRequest
    {
        public Guid DragId { get; set; }

        public DateTime DropDate { get; set; }

        public int Priority { get; set; }
    }
}
