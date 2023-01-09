using System;
using TDL.Infrastructure.Persistence.Base;

namespace TDL.Domain.Entities
{
    public class Todo : BaseEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public Guid ToDoCategoryId { get; set; }

        public virtual TodoCategory TodoCategory { get; set; }
    }
}
