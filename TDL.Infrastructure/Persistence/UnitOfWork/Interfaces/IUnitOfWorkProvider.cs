using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TDL.Infrastructure.Persistence.UnitOfWork.Interfaces
{
    public interface IUnitOfWorkProvider
    {
        /// <summary>
        /// Privide a new unit of work scope.
        /// </summary>
        /// <returns></returns>
        IUnitOfWorkScope Provide();

        IUnitOfWorkScope Provide(UnitOfWorkScopeOption scopeOption);

        IUnitOfWorkScope Provide(IsolationLevel isolationLevel);
    }
}
