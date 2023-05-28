using System;
using System.Collections.Generic;

namespace TDL.Services.Dto.Workspace
{
    public class GetWorkspaceDetailResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int TotalUser { get; set; }

        public IList<UserInfoDetail> Users { get; set; }
    }

    public class UserInfoDetail
    {
        public Guid Id { get; set; }

        public string Img { get; set; }

        public string UserName { get; set; }
    }
}
