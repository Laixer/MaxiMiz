using Maximiz.Model;
using Maximiz.Model.Entity;
using Maximiz.Model.Protocol;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Maximiz.ServiceBus
{
    public class ServiceBusQueue
    {
        private static IConfiguration _configuration;

        /// <summary>
        /// Returns a new instance of <see cref="QueueClient"/> for the MaxiMiz service bus
        /// </summary>
        public static IQueueClient GetQueueClient => new QueueClient(_configuration.GetConnectionString("MaxiMizServiceBus"), "testqueue");

        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Sends a single <see cref="CreateOrUpdateObjectsMessage"></see> message to the MaxiMiz Service Bus.
        /// </summary>
        /// <param name="entity">The entity to manipulate</param>
        /// <param name="crudAction">The action to apply to the entity</param>
        /// <returns></returns>
        public static async Task SendObjectMessage(Entity entity, CrudAction crudAction)
        {
            var message = new CreateOrUpdateObjectsMessage(entity, crudAction);

            // Obtain the Queue client
            var qClient = GetQueueClient;

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                // Serialize the message
                formatter.Serialize(stream, message);

                // Send the message to the service bus
                await qClient.SendAsync(new Message(stream.ToArray()));
            }
        }

        /// <summary>
        /// Sends <see cref="CreateOrUpdateObjectsMessage"></see> messages for each entity in the given collection to the MaxiMiz Service Bus.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="crudAction"></param>
        /// <returns></returns>
        public static async Task SendObjectMessages(IEnumerable<Entity> entities, CrudAction crudAction)
        {
            foreach (Entity e in entities.AsParallel())
            {
                await SendObjectMessage(e, crudAction);
            }
        }

    }
}
