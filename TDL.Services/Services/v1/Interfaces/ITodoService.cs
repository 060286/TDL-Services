using System;
using System.Collections.Generic;
using System.Text;
using TDL.Domain.Entities;
using TDL.Services.Dto.MyDayPage;

namespace TDL.Services.Services.v1.Interfaces
{
    public interface ITodoService
    {
        GetMyDayItemDetailResponseDto GetTodoById(Guid todoId);

        void CreateSimpleTodo(CreateSimpleTodoRequestDto request);

        void UpdateTodo(UpdateTodoRequestDto request);

        IList<Todo> GetAllTodo();
    }
}
