using System;

namespace TDL.Services.Dto.Workspace
{
    public class CreateTodoWorkspaceRequestDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string FileName { get; set; }

        public DateTime? RemindedAt { get; set; }

        public Guid WorkspaceId { get; set; }
    }
}