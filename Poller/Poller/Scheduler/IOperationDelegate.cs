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
        /// Operation timeout.
        /// </summary>
        TimeSpan Timeout { get; }

        /// <summary>
        /// Run the registered operation.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        Task InvokeAsync(CancellationToken token);
    }
}
