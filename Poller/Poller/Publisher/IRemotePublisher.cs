using Poller.Scheduler;
using System.Threading;

namespace Poller.Publisher
{
    public interface IRemotePublisher
    {
        ScheduleCollection CreateSchedulerScheme(CancellationToken cancellationToken = default);
    }
}
