using System;
using TDL.Services.Dto.AllMyTask;

namespace TDL.Services.Services.v1.Interfaces
{
    public interface IAllMyTaskPageService
    {
        AllMyTaskListResponseDto GetAllTask(DateTime datetime);
    }
}
