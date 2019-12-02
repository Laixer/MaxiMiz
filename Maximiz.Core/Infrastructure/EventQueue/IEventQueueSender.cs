using Maximiz.Model.Protocol;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.EventQueue
{

    /// <summary>
    /// Contract for event queue functionality.
    /// </summary>
    public interface IEventQueueSender
    {

        /// <summary>
        /// Sends a <see cref="CreateOrUpdateObjectsMessage"/> to the event queue.
        /// </summary>
        /// <param name="message"><see cref="CreateOrUpdateObjectsMessage"/></param>
        /// <returns>True if successful, false if not</returns>
        Task<bool> SendMessageAsync(CreateOrUpdateObjectsMessage message);

        /// <summary>
        /// Sends a collection of <see cref="CreateOrUpdateObjectsMessage"/> objects to the event queue.
        /// </summary>
        /// <param name="messages"><see cref="IEnumerable{CreateOrUpdateObjectsMessage}"/></param>
        /// <returns>True if successful, false if not</returns>
        Task<bool> SendMessagesAsync(IEnumerable<CreateOrUpdateObjectsMessage> messages);

    }

}
