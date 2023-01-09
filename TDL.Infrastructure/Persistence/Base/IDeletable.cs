using System;

namespace TDL.Infrastructure.Persistence.Base
{
    public interface IDeletable
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedAt { get; set; }

        string DeletedBy { get; set; }
    }
}
