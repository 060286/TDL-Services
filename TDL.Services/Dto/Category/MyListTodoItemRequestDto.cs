using System;
using TDL.Infrastructure.Enums;

namespace TDL.Services.Dto.Category
{
    public class MyListTodoItemRequestDto
    {
        public SortType? SortType { get; set; }

        public SortBy? SortBy { get; set; }

        public Guid CategoryId { get; set; }
    }
}