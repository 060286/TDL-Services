using System;
using TDL.Infrastructure.Persistence.Base;

namespace TDL.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Color { get; set; }

        public Guid TodoId { get; set; }

        public virtual Todo Todo { get; set; }
    }
}
