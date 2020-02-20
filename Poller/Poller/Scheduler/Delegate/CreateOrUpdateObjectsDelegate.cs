using System.Threading;
using System.Threading.Tasks;
using Poller.Poller;

namespace Poller.Scheduler.Delegate
{
    public class CreateOrUpdateObjectsDelegate : OperationDelegate<IPollerCreateOrUpdateObjects>
    {
        public CreateOrUpdateObjectsDelegate(IPollerCreateOrUpdateObjects poller)
            : base(poller)
        {
        }

        protected override PollerContext BuildPollerContext()
        {
            var baseContext = base.BuildPollerContext();
            return new CreateOrUpdateObjectsContext(baseContext.RunCount, baseContext.LastRun);
        }

        protected override async Task InvokeDelegateAsync(IOperationContext context, CancellationToken token)
        {
            var operationContext = context as EventActivatorOperationContext;
            var pollerContext = BuildPollerContext() as CreateOrUpdateObjectsContext;

            pollerContext.Action = operationContext.EntityAction;
            pollerContext.Entity = operationContext.Entity;

            await _poller.CreateOrUpdateObjectsAsync(pollerContext, token);

            DigestPollerContext(pollerContext);
        }
    }
}
