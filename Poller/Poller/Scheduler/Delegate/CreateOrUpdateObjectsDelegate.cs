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

        public override Task InvokeAsync(CancellationToken token) => _poller.CreateOrUpdateObjectsAsync(token);
    }
}
