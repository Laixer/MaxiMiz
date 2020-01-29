using Maximiz.Model.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Commiting
{

    /// <summary>
    /// Contract for creating, updating and deleting a <see cref="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
    public interface ICommitter<TEntity>
        where TEntity : Entity
    {

        /// <summary>
        /// Creates a <see cref="TEntity"/> in our data store and returns it.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="TEntity"/></returns>
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken token);

        /// <summary>
        /// Updates a <see cref="TEntity"/> in our data store and returns it.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="TEntity"/></returns>
        Task<TEntity> Update(TEntity entity, CancellationToken token);

        /// <summary>
        /// Deletes a <see cref="TEntity"/> in our data store and returns it.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="TEntity"/></returns>
        Task<TEntity> Delete(TEntity entity, CancellationToken token);

    }
}
