using System;

namespace TDL.Services.Dto.Workspace
{
    public class DragDropTodoInWorkspaceRequestDto
    {
        public Guid TodoId { get; set; }

        public string SectionName { get; set; }

        public int Priority { get; set; }

        public int DroppableId { get; set; }
    }
}
