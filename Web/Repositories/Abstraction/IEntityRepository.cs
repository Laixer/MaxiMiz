using Maximiz.Model.Entity;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Abstraction
{

    /// <summary>
    /// Interface for a repository of entities.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimary">Primary key type</typeparam>
    public interface IEntityRepository<TEntity, TPrimary>
        where TEntity : Entity
    {

        /// <summary>
        /// Returns a specific entity
        /// </summary>
        /// <param name="id">The primary key of the entity to obtain</param>
        Task<TEntity> Get(TPrimary id);

        /// <summary>
        /// Create a new entity and return its created ID.
        /// </summary>
        Task<TPrimary> Create(TEntity entity);

        /// <summary>
        /// Update an existing entity. The entity should contain the updated fields.
        /// </summary>
        Task<TEntity> Update(TEntity entity);

        /// <summary>
        /// Delete a specific entity.
        /// </summary>
        Task Delete(TEntity entity);

    }

}
