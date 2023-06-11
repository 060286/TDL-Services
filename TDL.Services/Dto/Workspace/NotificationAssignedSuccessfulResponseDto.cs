using System;

namespace TDL.Services.Dto.Workspace
{
    public class NotificationAssignedSuccessfulResponseDto
    {
        public Guid WorkspaceId { get; set; }

        public string Message { get; set; }
    }
}
