using System;
using TDL.Infrastructure.Persistence.Base;

namespace TDL.Domain.Entities
{
    public class SubTask : BaseEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public bool IsCompleted { get; set; } = false;

        public Guid TodoId { get; set; }

        public virtual Todo Todo { get; set; }
    }
}
