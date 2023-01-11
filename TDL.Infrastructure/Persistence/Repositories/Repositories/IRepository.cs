using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TDL.Infrastructure.Persistence.Repositories.Repositories
{
    public interface IRepository<TEntity>
    {
        TEntity Get(Guid id);

        void Add(TEntity entity);

        void AddRange(IList<TEntity> entities);

        void Update(TEntity entity);

        void UpdateRange(IList<TEntity> entities);

        void Delete(TEntity entity);

        void DeleteRange(IList<TEntity> entities);

        void BulkInsertOrUpdate(IList<TEntity> entities);

        void BulkDelete(IList<TEntity> entities);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAll(bool isUntrackEntities);

        IQueryable<TEntity> GetAll(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeFn);

        IQueryable<TEntity> GetAll(bool isUntrackEntities, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeFn);

        IQueryable<TEntity> FromSqlRaw(string query, params object[] parameters);

        /// <summary>
        /// API for checking concurrent update.
        /// </summary>
        /// <param name="existing">Existing row version in database</param>
        /// <param name="incoming">Incoming row version from request</param>
        /// <param name="message">Message should be thrown when conflicting</param>
        void CheckRowVersion(long existing, long incoming, string message);
    }
}
