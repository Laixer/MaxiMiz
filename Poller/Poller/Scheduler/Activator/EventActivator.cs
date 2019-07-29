using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Poller.EventBus;

namespace Poller.Scheduler.Activator
{
    public class EventActivator : ActivatorBase
    {
        private readonly string ServiceBusConnectionString;
        private readonly IQueueClient queueClient;

        public EventActivator(IServiceProvider serviceProvider, IOperationDelegate operation, string queueName)
            : base(serviceProvider, operation)
        {
            var options = serviceProvider.GetRequiredService<IOptions<EventBusProviderOptions>>();
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            ServiceBusConnectionString = config.GetConnectionString(options.Value.ConnectionStringName);

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
