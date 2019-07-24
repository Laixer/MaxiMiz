using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Poller.Scheduler.Activator
{
    public class EventActivator : ActivatorBase
    {
        private const string ServiceBusConnectionString = "<your_connection_string>";
        private const string QueueName = "<your_queue_name>";
        private readonly IQueueClient queueClient;

        public EventActivator(IServiceProvider serviceProvider, IOperationDelegate operation)
            : base(serviceProvider, operation)
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, new MessageHandlerOptions(null));
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            ExecuteProvider();

            await Task.CompletedTask;
        }

        public override void Initialize(CancellationToken token)
        {
            CancellationToken = token;
        }
    }
}
