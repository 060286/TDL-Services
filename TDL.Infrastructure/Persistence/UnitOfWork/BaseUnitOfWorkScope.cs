using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading.Tasks;
using TDL.Infrastructure.Persistence.Context;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;

namespace TDL.Infrastructure.Persistence.UnitOfWork
{
    public abstract class BaseUnitOfWorkScope<TContext, TScope> : IBaseUnitOfWorkScope
        where TScope : class, IBaseUnitOfWorkScope
        where TContext: BaseDbContext
    {
        #region fields

        private TScope _savedScope;
        private bool _isCompleted;

        #endregion fields

        #region Properties

        protected bool IsRootScope { get; private set; }

        protected IDbContextTransaction CurrentTransaction { get; set; }

        /// <summary>
        /// The current scope for a specific thread.
        /// </summary>
        [field: ThreadStatic]
        public static TScope Current { get; private set; }

        public IBaseUnitOfWorkScope Parent => _savedScope;
        public abstract TContext DbContext { get; }

        #endregion Properties

        #region Public Method

        public void Complete()
        {
            _isCompleted = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        #endregion Public Method

        #region Protected Methods

        protected abstract void CreateContext(IsolationLevel isolationLevel);

        protected abstract void InheritContext();

        protected void InitializeScope(UnitOfWorkScopeOption scopeOption, IsolationLevel isolationLevel)
        {
            InitializeScopeInternal(Current == null ? UnitOfWorkScopeOption.RequiresNew : scopeOption, isolationLevel);
        }

        protected virtual void Dispose(bool disposiong)
        {
            if (!disposiong)
            {
                return;
            } 

            try
            {
                if(_isCompleted)
                {
                    DbContext.SaveChanges();

                    if(IsRootScope)
                    {
                        CurrentTransaction?.Commit();
                    }
                    else
                    {
                        if(IsRootScope)
                        {
                            CurrentTransaction?.Rollback();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new DataException("Unit of work error.", ex);
            }
            finally
            {
                if(IsRootScope)
                {
                    CurrentTransaction?.Dispose();
                    DbContext.Dispose();
                    Current = null;
                }
            }

            PopScope();
        }

        #endregion Protected Methods

        #region Private Methods 


        /// <summary>
        /// Initialize a new scope or inherit from the ambient scope(if any) and kêp all scopes in a scope stack.
        /// </summary>
        /// <param name="scopeOption"></param>
        /// <param name="isolationLevel"></param>
        private void InitializeScopeInternal(UnitOfWorkScopeOption scopeOption, IsolationLevel isolationLevel)
        {
            switch(scopeOption)
            {
                case UnitOfWorkScopeOption.Required:
                    InheritContext();
                    PushScope(this as TScope);
                    break;

                case UnitOfWorkScopeOption.RequiresNew:
                    IsRootScope = true;
                    CreateContext(isolationLevel);
                    PushScope(this as TScope);
                    break;

                default:
                    throw new InvalidOperationException("Unexpected value ScopeOption = " + scopeOption);
            }
        }

        /// <summary>
        /// Add a scope on top of the scope's stack, it will be removed
        /// durring Disposed
        /// </summary>
        /// <param name="scope">
        /// Scope to be pushed to the stack frame.
        /// </param>
        private void PushScope(TScope scope)
        {
            _savedScope = Current;
            Current = scope;
        }

        /// <summary>
        /// Pop the top most from the scope's stack and drop ít content
        /// because it's no longer needed
        /// </summary>
        private void PopScope()
        {
            Current = _savedScope;
            _savedScope = null;
        }

        #endregion Private Methods
    }
}
