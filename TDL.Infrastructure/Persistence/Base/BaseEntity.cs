using System;

namespace TDL.Infrastructure.Persistence.Base
{
    public class BaseEntity : IDeletable, IAuditable
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string DeletedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
