using System;
using System.Collections.Generic;
using System.Text;

namespace TDL.Domain.Entities
{
    public class UserWorkspace
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid WorkspaceId { get; set; }

        public virtual User User { get; set; }

        public virtual Workspace Workspace { get; set; }
    }
}
