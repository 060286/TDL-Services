using System.Collections.Generic;
using TDL.Services.Dto.Color;

namespace TDL.Services.Dto.NextSevenDayPage
{
    public class GetTodoNextSevenDayItemResponseDto
    {
        public string Title { get; set; }

        public string Category { get; set; }

        public bool HaveSubTask { get; set; }

        public ColorDto Priority { get; set; }

        public bool IsCompleted { get; set; }
    }
}
