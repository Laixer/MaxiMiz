using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poller.Scheduler
{
    public interface IOperationDelegate
    {
        TimeSpan Interval { get; }

        Task InvokeAsync(CancellationToken token);
    }
}
