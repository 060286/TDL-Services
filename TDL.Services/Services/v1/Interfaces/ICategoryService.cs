using System;
using System.Collections.Generic;
using TDL.Services.Dto.Category;

namespace TDL.Services.Services.v1.Interfaces
{
    public interface ICategoryService
    {
        IList<MyListCategoryItem> GetCategoryByUserName(string userName);

        void CreateCategoryItem(CreateCategoryItemRequestDto request);

        IList<MyListTodoItemResponse> GetMyListTodosItem(MyListTodoItemRequestDto request, string userName);

        void CreateSubtask(CreateSubtaskRequestDto request);
    }
}