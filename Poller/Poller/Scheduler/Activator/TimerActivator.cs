using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Poller.Scheduler.Activator
{
    /// <summary>
    /// Used to activate a timer.
    /// TODO Better doc.
    /// </summary>
    public class TimerActivator : ActivatorBase
    {
        private readonly Timer _timer;
        private readonly TimeSpan _interval;

        /// <summary>
        /// Constructor with dependency injection.
        /// </summary>
        /// <param name="serviceProvider">Service provider</param>
        /// <param name="operation">The operation</param>
        /// <param name="interval">The interval</param>
        public TimerActivator(IServiceProvider serviceProvider, IOperationDelegate operation, TimeSpan interval)
            : base(serviceProvider, operation)
        {
            _timer = new Timer(TimerCallback);
            _interval = interval;
        }

        public override void Initialize(CancellationToken token)
        {
            CancellationToken = token;
            Scheduler.ScheduleTimer(_timer, _interval, true);
        }

        /// <summary>
        /// Called when the timer exceeds.
        /// </summary>
        /// <param name="_">Disposed object TODO is this correct</param>
        private async void TimerCallback(object _)
        {
            await ExecuteProviderAsync();

            if (!CancellationToken.IsCancellationRequested)
            {
                // TODO: reschedule
                Scheduler.ScheduleTimer(_timer, _interval, true);

                Logger.LogDebug($"Rerun {OperationName} in ~{_interval}");
            }
        }

        /// <summary>
        /// Called upon graceful shutdown.
        /// </summary>
        public override void Dispose() => _timer.Dispose();
    }
}
