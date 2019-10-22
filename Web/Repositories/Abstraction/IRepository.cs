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
    /// TODO Implement cancellation token?
    /// </summary>
    /// <typeparam name="TEntityModel">Entity type</typeparam>
    public interface IRepository<TEntity>
        where TEntity : Entity
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
        Task<IEnumerable<TEntity>> GetAll(int page);

        /// <summary>
        /// This gets called upon (tactically timed) repository creation. This
        /// then retrieves all separate implemented queries from the database to
        /// be able to present their results immedeatly.
        /// </summary>
        /// <returns>Tasks</returns>
        Task QueryOnLoad();

    }
}
