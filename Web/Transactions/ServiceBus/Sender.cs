using Laixer.Library.Injection.ServiceBus;
using Maximiz.Model;
using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using Maximiz.Model.Protocol;
using System;
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
        private IServiceBusSender serviceBusSender;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="sender">The service bus sender</param>
        public Sender(IServiceBusSender sender)
        {
            serviceBusSender = sender;
        }

        /// <summary>
        /// Sends an entity through the service bus.
        /// This simply ignores anything that is not a:
        /// - <see cref="Campaign"/>
        /// - <see cref="AdItem"/>
        /// </summary>
        /// <param name="entity">The entity to send</param>
        /// <param name="account">The account the entity belongs to</param>
        /// <returns>Task</returns>
        public async Task SendAsync(Entity entity, Account account, CrudAction crudAction)
        {
            if (entity.GetType() != typeof(Campaign) && entity.GetType() != typeof(AdItem)) { return; }

            var message = null as CreateOrUpdateObjectsMessage;
            switch (account.Publisher)
            {
                case Publisher.Taboola:
                    message = ForTaboola(entity, account, crudAction);
                    break;
                default:
                    throw new NotImplementedException("Other publishers than Taboola are not supported yet");
            }
            await serviceBusSender.SendMessageAsync(message);
        }

        /// <summary>
        /// Constructs a crud message for Taboola.
        /// </summary>
        /// <param name="entity">The entity to crud</param>
        /// <param name="account">The corresponding taboola acocunt</param>
        /// <param name="crudAction">What to do with the entity</param>
        /// <returns><see cref="CreateOrUpdateObjectsMessage"/></returns>
        private CreateOrUpdateObjectsMessage ForTaboola(Entity entity, Account account, CrudAction crudAction)
        {
            var entities = new Entity[] { account, entity };
            return new CreateOrUpdateObjectsMessage(entities, crudAction); 
        }
    }
}
