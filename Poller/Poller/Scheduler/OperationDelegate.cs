using System;
using System.Threading;
using System.Threading.Tasks;
using Poller.Poller;

namespace Poller.Scheduler
{
    public abstract class OperationDelegate<TPoller> : IOperationDelegate
        where TPoller : IPoller
    {
        protected TPoller _poller;

        public TimeSpan Interval { get; protected set; }

        public OperationDelegate(TPoller poller, TimeSpan timeSpan)
        {
            Interval = timeSpan;
            _poller = poller;
        }

        public abstract Task InvokeAsync(CancellationToken token);
    }
}
