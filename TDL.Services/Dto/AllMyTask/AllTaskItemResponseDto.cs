using System;
using System.Collections.Generic;
using TDL.Domain.Entities;
using TDL.Services.Dto.Color;
using TDL.Services.Dto.MyDayPage;

namespace TDL.Services.Dto.AllMyTask
{
    public class AllTaskItemResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string CategoryName { get; set; }
        public bool IsCompleted { get; set; }

        public string Description { get; set; }

        public DateTime TodoDate { get; set; }

        public string DateRemind { get; set; }

        public string Status { get; set; }

        public int Priority { get; set; }

        public bool IsPinned { get; set; }

        public string TodoCategory { get; set; }

        //public IFormFile AttachmentFile { get; set; }

        public string FileName { get; set; }

        public DateTime? RemindedAt { get; set; }

        public bool IsArchieved { get; set; } = false;

        public IList<SubTaskResponse> SubTasks { get; set; }

        // Tag 
        public ColorDto Tag { get; set; }

        public static AllTaskItemResponseDto FromTodo(Todo todo)
        {
            return new AllTaskItemResponseDto
            {
                Id = todo.Id,
                Title = todo.Title,
                IsCompleted = todo.IsCompleted,
                TodoDate = todo.TodoDate,
                Description = todo.Description,
            };
        }
    }
}
