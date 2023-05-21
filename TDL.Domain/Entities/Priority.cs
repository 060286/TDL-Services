using System;
using TDL.Infrastructure.Persistence.Base;

namespace TDL.Domain.Entities
{
    public class Priority : BaseEntity, IKey<Guid>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string BackgroundColor { get; set; }

        public string TextColor { get; set; }
    }
}