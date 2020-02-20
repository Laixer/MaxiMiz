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

        public TimeSpan Timeout { get; protected set; }

        public event EventHandler OnSlidingWindowChange;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="poller">Instance of an <see cref="IPoller"/>.</param>
        public OperationDelegate(TPoller poller) => _poller = poller;

        private void ProgressUpdate()
        {
            OnSlidingWindowChange?.Invoke(this, null);
        }

        /// <summary>
        /// Build the poller context from operation delegate.
        /// </summary>
        /// <returns><see cref="PollerContext"/>.</returns>
        protected virtual PollerContext BuildPollerContext()
            => new PollerContext(runCount, runLast, ProgressUpdate);

        /// <summary>
        /// Update operation delegate from context.
        /// </summary>
        /// <param name="context"><see cref="PollerContext"/>.</param>
        protected virtual void DigestPollerContext(PollerContext context)
        {
            //
        }

        public void UnsubscribeAllEvents()
        {
            OnSlidingWindowChange = null;
        }

        /// <summary>
        /// Run the registered operation.
        /// </summary>
        /// <param name="context">Operation context.</param>
        /// <param name="token">Cancellation token.</param>
        public async Task InvokeAsync(IOperationContext context, CancellationToken token)
        {
            await InvokeDelegateAsync(context, token);

            runCount++;
            runLast = DateTime.Now;
        }

        /// <summary>
        /// Run the registered operation.
        /// </summary>
        /// <param name="context">Operation context.</param>
        /// <param name="token">Cancellation token.</param>
        protected abstract Task InvokeDelegateAsync(IOperationContext context, CancellationToken token);
    }
}
