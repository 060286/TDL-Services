using System;
using System.Collections.Generic;
using System.Text;
using TDL.Services.Dto.MyDayPage;

namespace TDL.Services.Services.v1.Interfaces
{
    public interface ITodoService
    {
        GetMyDayItemDetailResponseDto GetTodoById(Guid todoId);
    }
}
