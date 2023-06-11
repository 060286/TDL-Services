using System;

namespace TDL.Services.Dto.Workspace
{
    public class SearchUserInWorkspaceRequestDto
    {
        public Guid WorkspaceId { get; set; }

        public string Keyword { get; set; }
    }
}
