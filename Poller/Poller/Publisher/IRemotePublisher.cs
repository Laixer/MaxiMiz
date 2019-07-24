using System.Threading;
using Poller.Scheduler.Activator;
using System.Collections.Generic;

namespace Poller.Publisher
{
    public interface IRemotePublisher
    {
        IEnumerable<ActivatorBase> GetActivators(CancellationToken cancellationToken = default);
    }
}
