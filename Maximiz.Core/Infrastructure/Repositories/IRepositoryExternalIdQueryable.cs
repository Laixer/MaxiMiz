using Maximiz.Model.Entity;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.Repositories
{

    /// <summary>
    /// Contract for searching for an entity based on its external id.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepositoryExternalIdQueryable<TEntity>
        where TEntity : Entity
    {

        /// <summary>
        /// Gets a single item from our data store.
        /// </summary>
        /// <param name="externalId">External datastore id</param>
        /// <returns><see cref="TEntity"/></returns>
        Task<TEntity> GetFromExternalIdAsync(string externalId);

    }
}
