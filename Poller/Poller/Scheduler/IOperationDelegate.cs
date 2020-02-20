using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poller.Scheduler
{
    public interface IOperationDelegate
    {
        /// <summary>
        /// Timeout after which the operation is canceled.
        /// </summary>
        TimeSpan Timeout { get; }

        /// <summary>
        /// Timeout window is changed event.
        /// </summary>
        event EventHandler OnSlidingWindowChange;

        /// <summary>
        /// Remove all listeners from all event.
        /// </summary>
        void UnsubscribeAllEvents();

        /// <summary>
        /// Run the registered operation.
        /// </summary>
        /// <param name="context">Operation context.</param>
        /// <param name="token">Cancellation token.</param>
        Task InvokeAsync(IOperationContext context, CancellationToken token);
    }
}
