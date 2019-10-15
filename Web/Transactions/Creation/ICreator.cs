using Maximiz.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Transactions.Creation
{

    /// <summary>
    /// Interface for entity creation in our database.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICreator<TEntity>
        where TEntity : Entity
    {

        /// <summary>
        /// Creates an entity of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The created entity as returned</returns>
        Task<TEntity> Create(TEntity entity);

        /// <summary>
        /// Creates a list of entities in our database.
        /// </summary>
        /// <param name="entities">The entities</param>
        /// <returns>All created entities</returns>
        Task<IEnumerable<TEntity>> CreateAll(IEnumerable<TEntity> entities);

    }
}
