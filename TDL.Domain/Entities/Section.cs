using System;
using System.Collections.Generic;
using TDL.Infrastructure.Persistence.Base;

namespace TDL.Domain.Entities
{
    public class Section : BaseEntity, IKey<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public Guid WorkspaceId { get; set; }

        public virtual IList<Todo> Todos { get; set; }

        public virtual Workspace Workspace { get; set; }
    }
}
