using System;

namespace Poller.Poller
{
    public class PollerContext
    {
        protected Action _progressCallback;

        public TimeSpan Interval { get; set; }
        public int RunCount { get; }
        public DateTime? LastRun { get; }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="runCount">Number of successful runs.</param>
        /// <param name="lastRun">Last sucessful run.</param>
        public PollerContext(int runCount, DateTime? lastRun, Action progressCallback = null)
        {
            RunCount = runCount;
            LastRun = lastRun;
            _progressCallback = progressCallback;
        }

        /// <summary>
        /// Notify the runner service of the operation progress.
        /// </summary>
        public virtual void MarkProgress() => _progressCallback?.Invoke();
    }
}
