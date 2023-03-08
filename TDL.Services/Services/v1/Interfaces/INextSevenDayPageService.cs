using System;
using TDL.Services.Dto.NextSevenDayPage;

namespace TDL.Services.Services.v1.Interfaces
{
    public interface INextSevenDayPageService
    {
        GetTodoNextSevenDayResponseDto GetNextSevenDay(GetTodoNextSevenDayRequestDto request, string userName);
    }
}