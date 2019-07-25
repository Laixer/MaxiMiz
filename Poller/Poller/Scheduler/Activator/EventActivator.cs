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

        public EventActivator(IServiceProvider serviceProvider, IOperationDelegate operation, string queueName)
            : base(serviceProvider, operation)
        {
            queueClient = new QueueClient(ServiceBusConnectionString, queueName);
            queueClient.RegisterMessageHandler(MessageCallbackAsync, new MessageHandlerOptions(ExceptionReceivedHandler));
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Logger.LogError(exceptionReceivedEventArgs.Exception.Message);

            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Logger.LogTrace($"Endpoint: {context.Endpoint}");
            Logger.LogTrace($"Entity Path: {context.EntityPath}");
            Logger.LogTrace($"Executing Action: {context.Action}");

            return Task.CompletedTask;
        }

        private async Task MessageCallbackAsync(Message message, CancellationToken token)
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
