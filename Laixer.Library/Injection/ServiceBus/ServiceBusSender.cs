using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Laixer.Library.Injection.ServiceBus;
using Microsoft.Azure.ServiceBus;

namespace Maximiz.ServiceBus
{

    /// <summary>
    /// Handles sending messages to a service bus.
    /// </summary>
    public class ServiceBusSender : IServiceBusSender
    {

        /// <summary>
        /// Provides queue client connections for us.
        /// </summary>
        private readonly IServiceBusProvider _serviceBusProvider;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        public ServiceBusSender(IServiceBusProvider serviceProvider)
        {
            _serviceBusProvider = serviceProvider;
        }

        /// <summary>
        /// Send and object as message through our service bus.
        /// </summary>
        /// <param name="objectToSend">The object to send</param>
        /// <returns>Task</returns>
        public async Task SendMessageAsync(object objectToSend)
        {
            var client = _serviceBusProvider.GetQueueClient();
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, objectToSend);
                var message = new Message(stream.ToArray());
                await client.SendAsync(message);
            }
        }

        /// <summary>
        /// Send a collection of objects through the service bus. Each item in
        /// the list is sent through <see cref="SendMessageAsync(object)"/>.
        /// </summary>
        /// <param name="objectsToSend">The objects to send</param>
        /// <returns>Task</returns>
        public async Task SendMessagesAsync(IEnumerable<object> objectsToSend)
        {
            foreach (var obj in objectsToSend)
            {
                await SendMessageAsync(obj);
            }
        }
    }
}
