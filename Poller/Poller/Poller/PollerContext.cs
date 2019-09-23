using System;
using System.Threading;

namespace Poller.Poller
{

    /// <summary>
    /// Context for our poller object. This contains basic information about
    /// the status of our relevant poller.
    /// </summary>
    public class PollerContext
    {

        /// <summary>
        /// Action to invoke when we mark our progress. This is generally used
        /// to tell our runner service about the progress.
        /// </summary>
        protected Action _progressCallback;

        /// <summary>
        /// Number of successful runs.
        /// </summary>
        public int RunCount { get; }

        /// <summary>
        /// Last successful run.
        /// </summary>
        public DateTime? LastRun { get; }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="runCount">Number of successful runs.</param>
        /// <param name="lastRun">Last sucessful run.</param>
        /// <param name="progressCallback">Action to execute when we want to 
        /// mark our operation progress.</param>
        public PollerContext(int runCount, DateTime? lastRun, Action progressCallback = null)
        {
            RunCount = runCount;
            LastRun = lastRun;
            _progressCallback = progressCallback;
        }

        /// <summary>
        /// Notify the runner service of the operation progress. This throws
        /// if our cancellation token has been cancelled.
        /// </summary>
        public virtual void MarkProgress(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _progressCallback?.Invoke();
        }
    }
}
