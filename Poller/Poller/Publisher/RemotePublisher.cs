using System.Threading;
using Microsoft.Extensions.Logging;
using Poller.Scheduler;

namespace Poller.Publisher
{
    public abstract class RemotePublisher : IRemotePublisher
    {
        public ILogger Logger { get; }

        public RemotePublisher(ILogger<RemotePublisher> logger)
        {
            Logger = logger;
        }

        public abstract ScheduleCollection CreateSchedulerScheme(CancellationToken cancellationToken);
    }
}
