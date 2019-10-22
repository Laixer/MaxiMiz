using Maximiz.Model;
using Maximiz.Model.Entity;
using System.Threading.Tasks;

namespace Maximiz.Transactions.CreateUpdateDelete
{

    /// <summary>
    /// Contract for selecting and executing the correct <see cref="ICud{TEntity}"/>
    /// interface implementation to enable generic entity creation, updating and
    /// deleting.
    /// </summary>
    internal interface ICudProcessor
    {

        /// <summary>
        /// Processes a create, update or delete request for a given entity.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/></param>
        /// <param name="action"><see cref="CrudAction"/></param>
        /// <returns>The processed entity as returned from the database</returns>
        Task<Entity> ProcessOperationAsync(Entity entity, CrudAction action);

    }
}
