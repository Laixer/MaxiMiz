using Laixer.Library.Injection.ServiceBus;
using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Transactions.ServiceBus
{

    /// <summary>
    /// Encapsulation for creating a service bus message out of given entities
    /// and sending these messages to the service bus.
    /// </summary>
    public class Sender : ISender<Entity>
    {

        /// <summary>
        /// Provides service bus object sending for us.
        /// </summary>
        private IServiceBusSender _serviceBusSender;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="serviceBusSender">The service bus sender</param>
        public Sender(IServiceBusSender serviceBusSender)
        {
            _serviceBusSender = serviceBusSender;
        }

        /// <summary>
        /// Sends an entity through the service bus.
        /// </summary>
        /// <param name="entity">The entity to send</param>
        /// <returns>Task</returns>
        public async Task Send(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}
