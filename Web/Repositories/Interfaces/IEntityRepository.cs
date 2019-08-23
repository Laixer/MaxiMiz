using Maximiz.Model.Entity;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Interfaces
{
    /// <typeparam name="TEntity">Entity</typeparam>
    /// <typeparam name="TPrimary">Primary key</typeparam>
    public interface IEntityRepository<TEntity, TPrimary>
        where TEntity : Entity
    {
        /// <summary>
        /// Returns a specific entity
        /// </summary>
        /// <param name="id">The primary key of the entity to obtain</param>
        Task<TEntity> Get(TPrimary id);

        /// <summary>
        /// Create a new entity
        /// </summary>
        Task Create(TEntity entity);

        /// <summary>
        /// Update an existing entity
        /// </summary>
        Task Update(TEntity entity);

        /// <summary>
        /// Delete a specific entity
        /// </summary>
        Task Delete(TEntity entity);
    }
}
