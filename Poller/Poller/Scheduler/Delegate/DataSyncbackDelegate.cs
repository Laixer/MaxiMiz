using System;
using System.Threading;
using System.Threading.Tasks;
using Poller.Poller;

namespace Poller.Scheduler.Delegate
{
    public class DataSyncbackDelegate : OperationDelegate<IPollerDataSyncback>
    {
        public DataSyncbackDelegate(IPollerDataSyncback poller, TimeSpan timeSpan)
            : base(poller, timeSpan)
        {
        }

        public override Task InvokeAsync(CancellationToken token) => _poller.DataSyncbackAsync(token);
    }
}
