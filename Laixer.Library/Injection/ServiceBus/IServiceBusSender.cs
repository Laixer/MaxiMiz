using Laixer.Library.Injection.ServiceBus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Laixer.Library.Injection.ServiceBus
{

    /// <summary>
    /// Contract for sending messages to a service bus.
    /// </summary>
    public interface IServiceBusSender
    {

        /// <summary>
        /// Sends an object as a message through the service bus.
        /// </summary>
        /// <param name="objectToSend">The object to send</param>
        /// <returns>Task</returns>
        Task SendMessageAsync(object objectToSend);

        /// <summary>
        /// Sends a collection of objects as messages through the service bus.
        /// </summary>
        /// <param name="objectsToSend">The objects to send</param>
        /// <returns>Task</returns>
        Task SendMessagesAsync(IEnumerable<object> objectsToSend);

    }
}
