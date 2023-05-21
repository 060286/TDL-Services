using System;
using System.Collections.Generic;
using System.Text;

namespace TDL.Services.Dto.Workspace
{
    public class GetUserOfWorkspaceResponseDto
    {
        IList<UserOfWorkspaceInfo> Users = new List<UserOfWorkspaceInfo>();
    }

    public class UserOfWorkspaceInfo
    {
        public Guid UserId { get; set; }

        public string ImgUrl { get; set; }

        public string Email { get; set; }
    }
}
