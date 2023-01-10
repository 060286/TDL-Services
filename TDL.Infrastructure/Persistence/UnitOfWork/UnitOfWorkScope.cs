using Microsoft.EntityFrameworkCore;
using System.Data;
using TDL.Infrastructure.Persistence.Context;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;

namespace TDL.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWorkScope : BaseUnitOfWorkScope<BaseDbContext, UnitOfWorkScope>, IUnitOfWorkScope
    {
        private BaseDbContext _dbContext;
        private readonly IContextFactory<BaseDbContext> _contextFactory;

        #region Constructors

        public UnitOfWorkScope(IContextFactory<BaseDbContext> contextFactory)
            : this(contextFactory, UnitOfWorkScopeOption.Required, IsolationLevel.ReadCommitted)
        {
        }

        public UnitOfWorkScope(IContextFactory<BaseDbContext> contextFactory, UnitOfWorkScopeOption scopeOption, IsolationLevel isolationLevel)
        {
            _contextFactory = contextFactory;
            InitializeScope(scopeOption, isolationLevel);
        }

        #endregion Constructors

        #region Public Methods

        public void Evict<T>(T obj)
        {
            if (DbContext.Entry(obj).State != EntityState.Detached)
            {
                DbContext.Entry(obj).State = EntityState.Detached;
            }
        }

        public void Attach<T>(T obj)
        {
            if (obj != null && DbContext.Entry(obj).State == EntityState.Detached)
            {
                DbContext.Entry(obj).State = EntityState.Unchanged;
            }
        }

        public EntityState? GetState<T>(T obj)
        {
            if (obj != null)
            {
                return DbContext.Entry(obj).State;
            }

            return null;
        }

        public int ExecuteSqlRaw(string sql, int timeout = 30, params object[] parameters)
        {
            DbContext.Database.SetCommandTimeout(timeout);

            return DbContext.Database.ExecuteSqlRaw(sql, parameters);
        }

        #endregion Public Methods

        #region Override Methods

        public override BaseDbContext DbContext => _dbContext;

        protected override void CreateContext(IsolationLevel isolationLevel)
        {
            _dbContext = _contextFactory.Create();
            CreateTransaction(isolationLevel);
        }

        protected override void InheritContext()
        {
            _dbContext = Current.DbContext;
        }

        #endregion Override Methods

        #region Private Methods

        private void CreateTransaction(IsolationLevel isolationLevel)
        {
            CurrentTransaction = DbContext.Database.BeginTransaction(isolationLevel);
        }

        #endregion
    }
}
