using System;
using TDL.Infrastructure.Persistence.Context;
using TDL.Infrastructure.Persistence.UnitOfWork;

namespace TDL.Infrastructure.Persistence.Repositories.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected BaseDbContext DbContext => UnitOfWorkScope.Current.DbContext;

        public TEntity Get(Guid id)
        {
            return DbContext.Set<TEntity>().Find(id);
        }
    }
}
