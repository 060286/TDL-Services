using System;

namespace TDL.Services.Dto.Workspace
{
    public class CreateWorkspaceResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
