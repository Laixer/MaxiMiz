using Maximiz.Model;
using Maximiz.Model.Entity;
using System.Threading.Tasks;

namespace Maximiz.Transactions.ServiceBus
{

    /// <summary>
    /// Contract for sending entities to a service bus through messages.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity</typeparam>
    public interface ISender<TEntity>
        where TEntity : Entity
    {

        /// <summary>
        /// Sends an entity through the service bus.
        /// </summary>
        /// <param name="entity">The entity to send</param>
        /// <param name="account">The account the entity belongs to</param>
        /// <param name="crudAction">What to do with the entity</param>
        /// <returns>Task</returns>
        Task SendAsync(TEntity entity, Account account, CrudAction crudAction);

    }
}
