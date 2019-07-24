using System;
using System.Threading;
using System.Threading.Tasks;
using Poller.Poller;

namespace Poller.Scheduler
{
    public abstract class OperationDelegate<TPoller> : IOperationDelegate
        where TPoller : IPoller
    {
        private int runCount = 0;
        private DateTime? runLast;
        protected TPoller _poller;

        public TimeSpan Interval { get; protected set; }
        public TimeSpan Timeout { get; protected set; }

        public event EventHandler OnSlidingWindowChange;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="poller">Instance of an <see cref="IPoller"/>.</param>
        /// <param name="timeSpan">Operation invoke interval.</param>
        public OperationDelegate(TPoller poller, TimeSpan timeSpan)
        {
            Interval = timeSpan;
            _poller = poller;
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="poller">Instance of an <see cref="IPoller"/>.</param>
        /// <param name="milliseconds">Operation invoke interval in miliseconds.</param>
        public OperationDelegate(TPoller poller, double milliseconds)
            : this(poller, TimeSpan.FromMilliseconds(milliseconds))
        {
        }

        private void ProgressUpdate()
        {
            OnSlidingWindowChange?.Invoke(this, null);
        }

        /// <summary>
        /// Build the poller context from operation delegate.
        /// </summary>
        /// <returns><see cref="PollerContext"/>.</returns>
        protected virtual PollerContext BuildPollerContext()
            => new PollerContext(runCount, runLast, ProgressUpdate)
            {
                Interval = Interval,
            };

        /// <summary>
        /// Update operation delegate from context.
        /// </summary>
        /// <param name="context"></param>
        protected virtual void DigestPollerContext(PollerContext context)
        {
            Interval = context.Interval;
        }

        public void UnsubscribeAllEvents()
        {
            OnSlidingWindowChange = null;
        }

        public async Task InvokeAsync(CancellationToken token)
        {
            await InvokeDelegateAsync(token);

            runCount++;
            runLast = DateTime.Now;
        }

        protected abstract Task InvokeDelegateAsync(CancellationToken token);
    }
}
