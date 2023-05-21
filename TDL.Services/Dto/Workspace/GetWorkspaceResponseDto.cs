using System;

namespace TDL.Services.Dto.Workspace
{
    public class GetWorkspaceResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
