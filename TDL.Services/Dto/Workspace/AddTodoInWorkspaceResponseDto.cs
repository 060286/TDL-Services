using System;
using System.Collections.Generic;
using System.Text;
using TDL.Infrastructure.Enums;

namespace TDL.Services.Dto.Workspace
{
    public class AddTodoInWorkspaceResponseDto
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

        public Guid? CategoryId { get; set; }

        public Guid? WorkspaceId { get; set; }

        public Guid? SectionId { get; set; }

        public string Tag { get; set; } = TagDefinition.Priority.ToString();

        public DateTime TodoDate { get; set; }
    }
}
