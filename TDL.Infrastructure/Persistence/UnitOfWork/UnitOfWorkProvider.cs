using System.Data;
using TDL.Infrastructure.Persistence.Context;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;

namespace TDL.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWorkProvider : IUnitOfWorkProvider
    {
        private readonly IContextFactory<BaseDbContext> _contextFactory;

        public UnitOfWorkProvider(IContextFactory<BaseDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IUnitOfWorkScope Provide()
        {
            return Provide(UnitOfWorkScopeOption.Required);
        }

        public IUnitOfWorkScope Provide(UnitOfWorkScopeOption scopeOption)
        {
            return new UnitOfWorkScope(_contextFactory, scopeOption, IsolationLevel.ReadCommitted);
        }

        public IUnitOfWorkScope Provide(IsolationLevel isolationLevel)
        {
            return new UnitOfWorkScope(_contextFactory, UnitOfWorkScopeOption.Required, isolationLevel);
        }
    }
}
