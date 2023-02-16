using System;
using System.Collections.Generic;
using TDL.Infrastructure.Enums;

namespace TDL.Services.Dto.MyDayPage
{
    public class UpdateTodoRequestDto
    {
        public Guid Id { get; set; }

        public string Tittle { get; set; }

        public DateTime? RemindedAt { get; set; }

        public Guid TodoCategoryId { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsArchieved { get; set; }

        public string Description { get; set; }

        public IList<string> SubTasks { get; set; }

        public IList<TagDefination> Tags { get; set; }
    }
}
