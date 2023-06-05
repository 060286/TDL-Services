using System;

namespace TDL.Services.Dto.Workspace
{
    public class AssignUserRequestDto
    {
        public string Email { get; set; }

        public Guid TodoId { get; set; }
    }
}
