﻿using System;
using TDL.Domain.Entities;

namespace TDL.Services.Dto.AllMyTask
{
    public class AllTaskItemResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string CategoryName { get; set; }
        public bool IsCompleted { get; set; }

        public DateTime TodoDate { get; set; }
        
        public static AllTaskItemResponseDto FromTodo(Todo todo)
        {
            return new AllTaskItemResponseDto
            {
                Id = todo.Id,
                Title = todo.Title,
                IsCompleted = todo.IsCompleted,
                TodoDate = todo.TodoDate,
            };
        }
    }
}
