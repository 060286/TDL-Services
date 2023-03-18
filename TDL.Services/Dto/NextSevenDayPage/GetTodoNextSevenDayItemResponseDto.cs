using System;
using System.Collections.Generic;
using TDL.Services.Dto.Color;

namespace TDL.Services.Dto.NextSevenDayPage
{
    public class GetTodoNextSevenDayItemResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public bool HaveSubTask { get; set; }

        public bool IsCompleted { get; set; }

        public int Priority { get; set; }

        public ColorDto Tag { get; set; }
    }
}
