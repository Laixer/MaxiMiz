using System;
using System.Threading;
using System.Threading.Tasks;
using Poller.Poller;

namespace Poller.Scheduler.Delegate
{
    public class RefreshAdvertisementDataDelegate : OperationDelegate<IPollerRefreshAdvertisementData>
    {
        public RefreshAdvertisementDataDelegate(IPollerRefreshAdvertisementData poller, TimeSpan timeSpan)
            : base(poller, timeSpan)
        {
        }

        public override async Task InvokeAsync(CancellationToken token)
        {
            var pollerContext = new PollerContext
            {
                Interval = Interval,
            };

            await _poller.RefreshAdvertisementDataAsync(pollerContext, token);

            Interval = pollerContext.Interval;
        }
    }
}
