using System.Collections;
using System.Collections.Generic;

namespace TDL.Services.Dto.NextSevenDayPage
{
    public class GetTodoNextSevenDayResponseDto
    {
        public IList<GetTodoNextSevenDayItemResponseDto> DayOne { get; set; }

        public IList<GetTodoNextSevenDayItemResponseDto> DayTwo { get; set; }

        public IList<GetTodoNextSevenDayItemResponseDto> DayThree { get; set; }
        
        public IList<GetTodoNextSevenDayItemResponseDto> DayFour { get; set; }
        
        public IList<GetTodoNextSevenDayItemResponseDto> DayFive { get; set; }
        
        public IList<GetTodoNextSevenDayItemResponseDto> DaySix { get; set; }
        
        public IList<GetTodoNextSevenDayItemResponseDto> DaySeven { get; set; }
    }
}