using Maximiz.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Commiting
{

    /// <summary>
    /// Contract for committing <see cref="TEntity"/> data to our data store in bulk.
    /// </summary>
    /// <typeparam name="TEntity"><see cref="TEntity"/></typeparam>
    public interface IBulkCommitter<TEntity>
        where TEntity : Entity
    {

        /// <summary>
        /// Commits a collection of <see cref="TEntity"/>s to our data store.
        /// </summary>
        /// <param name="entities"><see cref="IEnumerable{TEntity}"/></param>
        /// <returns>True if successful, false if not</returns>
        Task<bool> UpdateBulkAsync(IEnumerable<TEntity> entities);

    }
}
