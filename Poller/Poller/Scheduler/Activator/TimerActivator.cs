using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Poller.Scheduler.Activator
{
    public class TimerActivator : ActivatorBase
    {
        private readonly Timer _timer;
        private readonly TimeSpan _interval;

        public TimerActivator(IServiceProvider serviceProvider, IOperationDelegate operation, TimeSpan interval)
            : base(serviceProvider, operation)
        {
            _timer = new Timer(TimerCallback);
            _interval = interval;
        }

        public override void Initialize(CancellationToken token)
        {
            CancellationToken = token;

            Scheduler.ScheduleTimer(_timer, _interval);
        }

        private async void TimerCallback(object _)
        {
            await ExecuteProviderAsync();

            if (!CancellationToken.IsCancellationRequested)
            {
                // TODO: reschedule
                Scheduler.ScheduleTimer(_timer, _interval);

                Logger.LogDebug($"Rerun {OperationName} in ~{_interval}");
            }
        }

        public override void Dispose() => _timer.Dispose();
    }
}
