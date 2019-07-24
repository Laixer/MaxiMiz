﻿using System;
using System.Threading;

namespace Poller.Poller
{
    public class PollerContext
    {
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
        public PollerContext(int runCount, DateTime? lastRun, Action progressCallback = null)
        {
            RunCount = runCount;
            LastRun = lastRun;
            _progressCallback = progressCallback;
        }

        /// <summary>
        /// Notify the runner service of the operation progress.
        /// </summary>
        public virtual void MarkProgress(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _progressCallback?.Invoke();
        }
    }
}