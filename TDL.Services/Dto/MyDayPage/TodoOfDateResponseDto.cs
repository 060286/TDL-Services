using System;
using System.Collections.Generic;
using System.Text;
using TDL.Services.Dto.Color;

namespace TDL.Services.Dto.MyDayPage
{
    public class TodoOfDateResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string CategoryName { get; set; }

        public string DateRemind { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public int Priority { get; set; }

        public bool IsPinned { get; set; }

        public string TodoCategory { get; set; }

        //public IFormFile AttachmentFile { get; set; }

        public string FileName { get; set; }

        public DateTime? RemindedAt { get; set; }

        public bool IsCompleted { get; set; } = false;

        public bool IsArchieved { get; set; } = false;

        public IList<SubTaskResponse> SubTasks { get; set; }

        // Tag 
        public ColorDto Tag { get; set; }

        public DateTime TodoDate { get; set; }
    }
}
 