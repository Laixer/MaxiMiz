using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Repositories
{

    /// <summary>
    /// Contract for an <see cref="Entity"/> repository.
    /// </summary>
    public interface IRepository<TEntity> : IRepositoryQueryable<TEntity>
        where TEntity : Entity
    {

        /// <summary>
        /// Gets a single item from our data store.
        /// </summary>
        /// <param name="id">Internal datastore id</param>
        /// <returns><see cref="TEntity"/></returns>
        Task<TEntity> GetAsync(Guid id);

        /// <summary>
        /// Get a list of <see cref="TEntity"/>s from our data store.
        /// </summary>
        /// <returns><see cref="IEnumerable{TEntity}"/></returns>
        /// <param name="page">Paging option</param>
        Task<IEnumerable<TEntity>> GetAllAsync(int page = 0);

    }

}
