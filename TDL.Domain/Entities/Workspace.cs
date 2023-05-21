using System;
using System.Collections.Generic;
using TDL.Infrastructure.Persistence.Base;

namespace TDL.Domain.Entities
{
    public class Workspace : BaseEntity, IKey<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public virtual IList<UserWorkspace> UserWorkspaces { get; set; }
        
        public virtual IList<Todo> Todos { get; set; }

        public virtual IList<Section> Sections { get; set; }
    }
}
