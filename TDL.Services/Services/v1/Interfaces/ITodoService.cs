using System;
using System.Collections.Generic;
using TDL.Domain.Entities;
using TDL.Services.Dto.MyDayPage;

namespace TDL.Services.Services.v1.Interfaces
{
    public interface ITodoService
    {
        GetMyDayItemDetailResponseDto GetTodoById(Guid todoId);

        TodoOfDateResponseDto CreateSimpleTodo(CreateSimpleTodoRequestDto request);

        void UpdateTodo(UpdateTodoRequestDto request);

        IList<Todo> GetAllTodo();

        IList<TodoOfDateResponseDto> GetTodoOfDate(DateTime dateTime);

        IList<SuggestionTodoItemResponse> GetSuggestTodoList(string keyword);
    }
}
