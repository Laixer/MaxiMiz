using Maximiz.Model.Entity;
using System.Threading.Tasks;

namespace Maximiz.Transactions.CreateUpdateDelete
{

    /// <summary>
    /// Interface as facade for entity create, update and delete in our database.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal interface ICud<TEntity>
        where TEntity : Entity
    {

        /// <summary>
        /// Creates an entity of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="entity">The entity to create</param>
        /// <returns>The created entity as returned</returns>
        Task<TEntity> Create(TEntity entity);

        /// <summary>
        /// Updates an entity of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>The updated entity as returned</returns>
        Task<TEntity> Update(TEntity entity);

        /// <summary>
        /// Deletes an entity of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <returns>The deleted entity as returned</returns>
        Task<TEntity> Delete(TEntity entity);

    }
}
