using Maximiz.Model.Protocol;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Core.Infrastructure.EventQueue
{

    /// <summary>
    /// Contract for event queue functionality.
    /// </summary>
    public interface IEventQueueSender
    {

        /// <summary>
        /// Sends a <see cref="OperationMessage"/> to the event queue.
        /// </summary>
        /// <param name="message"><see cref="OperationMessage"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        Task SendMessageAsync(OperationMessage message, CancellationToken token);

        /// <summary>
        /// Sends a collection of <see cref="OperationMessage"/> objects to the event queue.
        /// </summary>
        /// <param name="messages"><see cref="IEnumerable{OperationMessage}"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        Task SendMessagesAsync(IEnumerable<OperationMessage> messages, CancellationToken token);

    }

}
