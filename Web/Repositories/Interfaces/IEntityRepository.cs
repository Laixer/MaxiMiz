using Maximiz.Model.Entity;

namespace Maximiz.Repositories.Interfaces
{
    /// <typeparam name="TE">Entity</typeparam>
    /// <typeparam name="TP">Primary key</typeparam>
    public interface IEntityRepository<TE, TP> where TE: Entity
    {
        /// <summary>
        /// Returns a specific entity
        /// </summary>
        /// <param name="id">The primary key of the entity to obtain</param>
        TE Get(TP id);
        
        /// <summary>
        /// Create a new entity
        /// </summary>
        void Create(TE entity);

        /// <summary>
        /// Update an existing entity
        /// </summary>
        void Update(TE entity);

        /// <summary>
        /// Delete a specific entity
        /// </summary>
        void Delete(TE entity);
    }
}
