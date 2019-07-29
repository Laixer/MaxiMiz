using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Poller.EventBus;

namespace Poller.Scheduler.Activator
{
    public class EventActivator : ActivatorBase
    {
        private readonly IQueueClient queueClient;

        public EventActivator(IServiceProvider serviceProvider, IOperationDelegate operation, string queueName)
            : base(serviceProvider, operation)
        {
            var eventBus = serviceProvider.GetRequiredService<EventBusProvider>();

            queueClient = eventBus.QueueClient(queueName);
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

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);

        public override void Initialize(CancellationToken token)
        {
            CancellationToken = token;
        }

        public override void Dispose()
        {
            queueClient.CloseAsync().Wait();
        }
    }
}
