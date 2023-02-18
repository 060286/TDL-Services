using System;
using System.Collections.Generic;
using System.Text;

namespace TDL.Domain.Entities
{
    public class Workspace
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public IList<UserWorkspace> UserWorkspaces { get; set; }
        
        public virtual IList<Todo> Todos { get; set; }
    }
}
