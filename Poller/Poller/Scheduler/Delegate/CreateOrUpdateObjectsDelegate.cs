using System;
using System.Threading;
using System.Threading.Tasks;
using Poller.Poller;

namespace Poller.Scheduler.Delegate
{
    public class CreateOrUpdateObjectsDelegate : OperationDelegate<IPollerCreateOrUpdateObjects>
    {
        public CreateOrUpdateObjectsDelegate(IPollerCreateOrUpdateObjects poller, TimeSpan timeSpan)
            : base(poller, timeSpan)
        {
        }

        protected override async Task InvokeDelegateAsync(CancellationToken token)
        {
            var pollerContext = BuildPollerContext();

            await _poller.CreateOrUpdateObjectsAsync(BuildPollerContext(), token);

            DigestPollerContext(pollerContext);
        }
    }
}
