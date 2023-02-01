using System.Collections.Generic;

namespace TDL.Services.Dto.AllMyTask
{
    public class AllMyTaskListResponseDto
    {
        public IList<AllTaskItemResponseDto> AllTaskToday { get; set; }

        public IList<AllTaskItemResponseDto> AllTaskTomorrow { get; set; }

        public IList<AllTaskItemResponseDto> AllTaskUpComming { get; set; }
    }
}
