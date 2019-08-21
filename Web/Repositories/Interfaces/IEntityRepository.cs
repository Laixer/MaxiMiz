using Maximiz.Model.Entity;

namespace Maximiz.Repositories.Interfaces
{
    /// <typeparam name="TEntity">Entity</typeparam>
    /// <typeparam name="TPrimary">Primary key</typeparam>
    public interface IEntityRepository<TEntity, TPrimary> where TEntity: Entity
    {
        /// <summary>
        /// Returns a specific entity
        /// </summary>
        /// <param name="id">The primary key of the entity to obtain</param>
        TEntity Get(TPrimary id);
        
        /// <summary>
        /// Create a new entity
        /// </summary>
        void Create(TEntity entity);

        /// <summary>
        /// Update an existing entity
        /// </summary>
        void Update(TEntity entity);

        /// <summary>
        /// Delete a specific entity
        /// </summary>
        void Delete(TEntity entity);
    }
}
