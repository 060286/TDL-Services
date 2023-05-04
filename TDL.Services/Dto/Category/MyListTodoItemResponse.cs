using System;
using System.Collections.Generic;
using TDL.Infrastructure.Enums;
using TDL.Services.Dto.Color;
using TDL.Services.Dto.MyDayPage;

namespace TDL.Services.Dto.Category
{
    public class MyListTodoItemResponse
    {
        public string CategoryName { get; set; }

        public IList<MyListTodoItem> Todos { get; set; }
    }

    public class MyListTodoItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public int Priority { get; set; }

        public string FileName { get; set; }

        public DateTime? RemindedAt { get; set; }

        public bool IsCompleted { get; set; } = false;

        public bool IsArchieved { get; set; } = false;

        public Guid CategoryId { get; set; }

        public Guid? WorkspaceId { get; set; }

        public string CategoryName { get; set; }

        public ColorDto Tag { get; set; }

        public int? CompletedSubtask { get; set; }

        public int? TotalSubtask { get; set; }

        public IList<SubTaskResponse> SubTasks { get; set; }

        public DateTime TodoDate { get; set; }
    }
}

