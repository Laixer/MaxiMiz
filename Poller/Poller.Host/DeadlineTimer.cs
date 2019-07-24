using System;
using System.Threading;

namespace Poller.Host
{
    /// <summary>
    /// A timer that cancels the tokensource when time up.
    /// </summary>
    public class DeadlineTimer : IDisposable
    {
        private readonly Timer _timer;
        private readonly CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="cancellationTokenSource">The cancelable token source.</param>
        public DeadlineTimer(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _timer = new Timer(TimerCallback);
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="cancellationTokenSource">The cancelable token source.</param>
        /// <param name="timer">Timer which is set for cancelation.</param>
        public DeadlineTimer(CancellationTokenSource cancellationTokenSource, Timer timer)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _timer = timer;
        }

        /// <summary>
        /// Callback to initiate the actions in the deadline operation.
        /// </summary>
        private void TimerCallback(object _) => CancelTokenSource();

        /// <summary>
        /// Changes the start time and the interval between method invocations for a timer.
        /// </summary>
        /// <param name="dueTime">The amount of time to delay before invoking the callback method.</param>
        /// <param name="period">The time interval between invocations of the callback method.</param>
        /// <returns>True if the timer was successfully updated; otherwise, false.</returns>
        public bool Change(TimeSpan dueTime, TimeSpan period) => _timer.Change(dueTime, period);

        /// <summary>
        /// Call cancel on the token.
        /// </summary>
        protected virtual void CancelTokenSource() => _cancellationTokenSource.Cancel();

        /// <summary>
        /// Dispose unmanaged resources.
        /// </summary>
        public void Dispose() => _timer.Dispose();
    }
}
