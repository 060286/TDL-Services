using System;
using System.ComponentModel.DataAnnotations.Schema;
using TDL.Infrastructure.Persistence.Base;

namespace TDL.Domain.Entities
{
    public class Notification : BaseEntity, IKey<Guid>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public bool IsViewed { get; set; }

        public string Content { get; set; }

        public string Url { get; set; }

        public Guid? OwnerId { get; set; }

        public Guid? ActorId { get; set; }

        [ForeignKey("OwnerId")]
        public User Owner { get; set; }

        [ForeignKey("ActorId")]
        public User Actor { get; set; }
    }
}
