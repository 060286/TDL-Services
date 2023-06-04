using System;

namespace TDL.Services.Dto.Workspace
{
    public class AddTodoIntoWorkspaceRequestDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime AddDate { get; set; }

        public Guid? WorkspaceId { get; set; }

        public Guid? SectionId { get; set; }

        public string SectionName { get; set; }

        public int Position { get; set; }
    }
}
