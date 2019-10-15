using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Transactions.ServiceBus
{

    /// <summary>
    /// Contract for sending entities to a service bus through messages.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISender<TEntity>
        where TEntity : Entity
    {

        /// <summary>
        /// Sends an entity through the service bus.
        /// </summary>
        /// <param name="entity">The entity to send</param>
        /// <returns>Task</returns>
        Task Send(TEntity entity);

    }
}
