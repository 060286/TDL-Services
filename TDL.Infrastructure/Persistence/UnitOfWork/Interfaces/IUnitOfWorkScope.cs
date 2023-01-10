using Microsoft.EntityFrameworkCore;
using TDL.Infrastructure.Persistence.Context;

namespace TDL.Infrastructure.Persistence.UnitOfWork.Interfaces
{
    public interface IUnitOfWorkScope : IBaseUnitOfWorkScope
    {
        BaseDbContext DbContext { get; }

        /// <summary>
        /// Evict a persisted object out of the current scope context.
        /// This means EF will no longer generate any SQL statements for this object if it has changed
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object
        /// </typeparam>
        /// <param name="obj">
        /// The object that will be evicted
        /// </param>
        void Evict<T>(T obj);

        /// <summary>
        /// Attach a detached object out of the current scope context
        /// This means EF wil generate SQL statements if this object has changed.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object.
        /// </typeparam>
        /// <param name="obj">
        /// The object that will be attached
        /// </param>
        void Attach<T>(T obj);

        /// <summary>
        /// Get the current state of and object 
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object.
        /// </typeparam>
        /// <param name="obj">
        /// The object that needs to get state 
        /// </param>
        /// <returns></returns>
        EntityState? GetState<T>(T obj);

        /// <summary>
		/// Execute raw sql query.
		/// </summary>
		/// <param name="sql">Raw query.</param>
		/// <param name="timeout">Execution time out.</param>
		/// <param name="parameters">Query params.</param>
		int ExecuteSqlRaw(string sql, int timeout = 30, params object[] parameters);
    }
}