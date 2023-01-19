using System;
using System.Collections.Generic;

namespace TDL.Services.Dto.Workspace
{
    public class CreateWorkspaceRequestDto
    {
        public List<Guid> UsersId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
