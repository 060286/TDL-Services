using System;

namespace TDL.Services.Dto.Workspace
{
    public class AddUserIntoWorkspaceRequestDto
    {
        public string Email { get; set; }

        public Guid WorkspaceId { get; set; }
    }
}
