using Maximiz.Core.Querying;
using Maximiz.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Repositories
{

    /// <summary>
    /// Contract for querying a <see cref="TEntity"/> in our data store.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepositoryQueryable<TEntity>
        where TEntity : Entity
    {

        /// <summary>
        /// Gets a list of <see cref="TEntity"/>s from our data store based on
        /// some given query.
        /// </summary>
        /// <param name="query"><see cref="QueryBase{TEntity}"/></param>
        /// <returns><see cref="IEnumerable{TEntity}"/></returns>
        Task<IEnumerable<TEntity>> GetAllAsync(QueryBase<TEntity> query);

        /// <summary>
        /// Gets the total number of existing items as result of a <paramref name="query"/>.
        /// </summary>
        /// <param name="query"><see cref="QueryBase{TEntity}"/></param>
        /// <returns>The total count</returns>
        Task<int> GetCountAsync(QueryBase<TEntity> query);

    }
}
