using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Maximiz.Model.Protocol;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Poller.EventBus;
using Poller.Helper;

namespace Poller.Scheduler.Activator
{
    public class EventActivator : ActivatorBase
    {
        private readonly IQueueClient queueClient;

        /// <summary>
        /// This subscribes the event activator to the specified service bus.
        /// </summary>
        /// <param name="serviceProvider">The service bus provider</param>
        /// <param name="operation">The operation delegate</param>
        /// <param name="queueName">The name of the queue (not the connection string)</param>
        public EventActivator(IServiceProvider serviceProvider, IOperationDelegate operation, string queueName)
            : base(serviceProvider, operation)
        {
            Logger = ServiceProvider.GetRequiredService<ILogger<EventActivator>>();

            queueName = queueName ?? throw new ArgumentNullException(nameof(queueName));

            queueClient = serviceProvider.GetRequiredService<EventBusProvider>().QueueClient(queueName);
            queueClient.RegisterMessageHandler(MessageCallbackAsync, new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 1,
            });

            Logger.LogDebug($"Event activator is listening on service bus");
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Logger.LogError(exceptionReceivedEventArgs.Exception.Message);
            Logger.LogTrace(exceptionReceivedEventArgs.Exception.StackTrace);

            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Logger.LogTrace($"Queue: {context.EntityPath}");
            Logger.LogTrace($"Action: {context.Action}");

            return Task.CompletedTask;
        }

        private async Task MessageCallbackAsync(Message message, CancellationToken token)
        {
            Logger.LogTrace($"Message ID: {message.MessageId}");
            Logger.LogTrace($"Message Sequence: {message.SystemProperties.SequenceNumber}");

            // TODO: Protocol operations should not be handeled here
            using (var stream = new MemoryStream(message.Body))
            {
                var protocolMessage = BinarySerializer.Deserialize<CreateOrUpdateObjectsMessage>(stream);
                if (protocolMessage.Header[0] != CreateOrUpdateObjectsMessage.Protocol.Header[0])
                {
                    throw new Exception("Unknown message format"); // TODO: Throw useful exception
                }
                if (protocolMessage.Version != CreateOrUpdateObjectsMessage.Protocol.Version)
                {
                    throw new Exception("Message version mismatch"); // TODO: Throw useful exception
                }
                if (protocolMessage.Entity.Length != protocolMessage.EntityLength)
                {
                    throw new Exception("Message corrupted"); // TODO: Throw useful exception
                }

                await ExecuteProviderAsync(new EventActivatorOperationContext
                {
                    Entity = protocolMessage.Entity,
                    EntityAction = protocolMessage.EntityAction,
                });
            }

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

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
