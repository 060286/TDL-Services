using System;
using System.Collections.Generic;
using TDL.Infrastructure.Persistence.Base;

namespace TDL.Domain.Entities
{
    public class TodoCategory : BaseEntity, IKey<Guid>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual IList<Todo> Todos { get; set; }
    }
}
