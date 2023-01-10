using System;
using System.Collections.Generic;
using System.Text;

namespace TDL.Infrastructure.Persistence.UnitOfWork
{
    public enum UnitOfWorkScopeOption
    {
        /// <summary>
        /// Use an ambient scope(if any) or create a new scope.
        /// </summary>
        Required, 

        /// <summary>
        /// Always create a new scope
        /// </summary>
        RequiresNew
    }
}
