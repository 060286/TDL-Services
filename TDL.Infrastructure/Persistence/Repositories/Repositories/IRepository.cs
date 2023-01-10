using System;

namespace TDL.Infrastructure.Persistence.Repositories.Repositories
{
    public interface IRepository<TEntity>
    {
        TEntity Get(Guid id);
    }
}
