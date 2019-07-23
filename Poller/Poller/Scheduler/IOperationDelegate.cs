using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poller.Scheduler
{
    public interface IOperationDelegate
    {
        /// <summary>
        /// Next interval when the operation must be called.
        /// </summary>
        TimeSpan Interval { get; }

        /// <summary>
        /// Timeout after which the operation is canceled.
        /// </summary>
        TimeSpan Timeout { get; }

        /// <summary>
        /// Timeout window is changed event.
        /// </summary>
        event EventHandler OnSlidingWindowChange;

        /// <summary>
        /// Run the registered operation.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        Task InvokeAsync(CancellationToken token);
    }
}
