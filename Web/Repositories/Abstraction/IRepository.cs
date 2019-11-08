using Maximiz.Database.Querying;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Abstraction
{

    /// <summary>
    /// Interface base for a repository, only responsible for performing get 
    /// operations.
    /// TODO Revise inheritance with ugly enum.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity</typeparam>
    /// <typeparam name="TQuery">The type of query object</typeparam>
    public interface IRepository<TEntity, TQuery, TColumn>
        where TEntity : Entity
        where TQuery : QueryBase<TColumn>
        where TColumn : Enum
    {

        /// <summary>
        /// Gets a single entity based on its internal id.
        /// </summary>
        /// <param name="id">The internal id</param>
        /// <returns>The entity</returns>
        Task<TEntity> Get(Guid id);

        /// <summary>
        /// Returns all (in reality: max 100) entities.
        /// </summary>
        /// <param name="page">The page to retrieve</param>
        /// <returns>The entities</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(TQuery query, int page = 0);

        /// <summary>
        /// Returns the total amount of items in the data store based on some query.
        /// </summary>
        /// <param name="query">The query</param>
        /// <returns>Total item count</returns>
        Task<int> GetCount(TQuery query);

        /// <summary>
        /// Query the data store.
        /// </summary>
        /// <param name="query"><see cref="TQuery"/></param>
        /// <param name="page">The page number</param>
        /// <returns>List of queried <see cref="TEntity"/>s</returns>
        Task<IEnumerable<TEntity>> GetQueriedAsync(TQuery query, int page = 0);

    }
}
