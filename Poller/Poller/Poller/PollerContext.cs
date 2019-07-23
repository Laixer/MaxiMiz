using System;

namespace Poller.Poller
{
    public class PollerContext
    {
        public TimeSpan Interval { get; set; }
        public int RunCount { get; }
        public DateTime? LastRun { get; }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="runCount">Number of successful runs.</param>
        /// <param name="lastRun">Last sucessful run.</param>
        public PollerContext(int runCount, DateTime? lastRun)
        {
            RunCount = runCount;
            LastRun = lastRun;
        }

        /// <summary>
        /// Notify the runner service the callee is still alive.
        /// This reset the timeout counter from this point onwards.
        /// </summary>
        public virtual void MarkProgress()
        {
            //
        }
    }
}
