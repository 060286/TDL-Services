using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TDL.Infrastructure.Persistence.UnitOfWork.Interfaces
{
    public interface IBaseUnitOfWorkScope : IDisposable
    {
        IBaseUnitOfWorkScope Parent { get; }

        /// <summary>
        /// Complete the current scope and send all done works to the database.
        /// </summary>
        void Complete();

        /// <summary>
        /// Send all done works to the database
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// Send asynchoronously all done works to the database.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}
