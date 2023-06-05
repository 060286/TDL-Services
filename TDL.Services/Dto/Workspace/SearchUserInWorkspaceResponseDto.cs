using System;

namespace TDL.Services.Dto.Workspace
{
    public class SearchUserInWorkspaceResponseDto
    {
        public Guid Id { get; set; }

        public string Img { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
