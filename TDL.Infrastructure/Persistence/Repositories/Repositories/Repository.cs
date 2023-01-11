using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using TDL.Infrastructure.Persistence.Context;
using TDL.Infrastructure.Persistence.UnitOfWork;

namespace TDL.Infrastructure.Persistence.Repositories.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected BaseDbContext DbContext => UnitOfWorkScope.Current.DbContext;

        public void Add(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
        }

        public void AddRange(IList<TEntity> entities)
        {
            DbContext.Set<TEntity>().AddRange(entities);
        }

        public void BulkDelete(IList<TEntity> entities)
        {
            DbContext.BulkDelete(entities);
        }

        public void BulkInsertOrUpdate(IList<TEntity> entities)
        {
            DbContext.BulkInsertOrUpdate(entities);
        }

        public void CheckRowVersion(long existing, long incoming, string message)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
        }

        public void DeleteRange(IList<TEntity> entities)
        {
            DbContext.Set<TEntity>().RemoveRange(entities);
        }

        public IQueryable<TEntity> FromSqlRaw(string query, params object[] parameters)
        {
            return DbContext.Set<TEntity>().FromSqlRaw(query, parameters);
        }

        public TEntity Get(Guid id)
        {
            return DbContext.Set<TEntity>().Find(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return GetAll(false, null);
        }

        public IQueryable<TEntity> GetAll(bool isUntrackEntities)
        {
            return GetAll(isUntrackEntities, null);
        }

        public IQueryable<TEntity> GetAll(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeFn)
        {
            return GetAll(false, includeFn);
        }

        public IQueryable<TEntity> GetAll(bool isUntrackEntities, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeFn)
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>();

            if (isUntrackEntities)
            {
                query.AsNoTracking();
            }

            if (includeFn != null)
            {
                query = includeFn(query);
            }

            return query;
        }

        public void Update(TEntity entity)
        {
            DbContext.Set<TEntity>().Update(entity);
        }

        public void UpdateRange(IList<TEntity> entities)
        {
            DbContext.Set<TEntity>().UpdateRange(entities);
        }
    }
}
