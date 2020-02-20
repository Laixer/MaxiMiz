using System.Threading;
using System.Threading.Tasks;
using Poller.Poller;

namespace Poller.Scheduler.Delegate
{
    public class DataSyncbackDelegate : OperationDelegate<IPollerDataSyncback>
    {
        public DataSyncbackDelegate(IPollerDataSyncback poller)
            : base(poller)
        {
        }

        protected override async Task InvokeDelegateAsync(IOperationContext context, CancellationToken token)
        {
            var pollerContext = BuildPollerContext();

            await _poller.DataSyncbackAsync(pollerContext, token);

            DigestPollerContext(pollerContext);
        }
    }
}
