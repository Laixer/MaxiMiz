using System;
using System.Threading;

namespace Poller.Scheduler.Activator
{
    public class EventActivator : ActivatorBase
    {
        public EventActivator(IServiceProvider serviceProvider, IOperationDelegate operation)
            : base(serviceProvider, operation)
        {
        }

        public override void Initialize(CancellationToken token)
        {
            CancellationToken = token;
        }
    }
}
