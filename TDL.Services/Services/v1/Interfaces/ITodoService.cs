using System;
using System.Collections.Generic;
using TDL.Domain.Entities;
using TDL.Services.Dto.Color;
using TDL.Services.Dto.MyDayPage;

namespace TDL.Services.Services.v1.Interfaces
{
    public interface ITodoService
    {
        GetMyDayItemDetailResponseDto GetTodoById(Guid todoId);

        TodoOfDateResponseDto CreateSimpleTodo(CreateSimpleTodoRequestDto request, string userName);

        void UpdateTodo(UpdateTodoRequestDto request);

        IList<Todo> GetAllTodo();

        IList<TodoOfDateResponseDto> GetTodoOfDate(DateTime dateTime);

        IList<SuggestionTodoItemResponse> GetSuggestTodoList(string keyword);

        string ChangeCategoryTitle(ChangeTodoCategoryRequestDto request, string userName);

        void ArchieTodo(Guid id);

        Todo CompletedTodo(Guid id);

        int CountTaskNotCompleted(DateTime dateTime, string userName);

        void UpdateSubTaskStatus(Guid id);

        GetMyDayItemDetailResponseDto UpdateTodoTitle(Guid id,string title);

        GetMyDayItemDetailResponseDto UpdateTodoDescription(Guid id, string description);

        IList<ColorDto> GetTagList();
    }
}
