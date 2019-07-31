﻿using System;
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

        public EventActivator(IServiceProvider serviceProvider, IOperationDelegate operation, string queueName)
            : base(serviceProvider, operation)
        {
            Logger = ServiceProvider.GetRequiredService<ILogger<EventActivator>>();

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

            using (var stream = new System.IO.MemoryStream(message.Body))
            {
                var protocolMessage = BinarySerializer.Deserialize<CreateOrUpdateObjectsMessage>(stream);
                if (protocolMessage.Header[0] != CreateOrUpdateObjectsMessage.Protocol.Header[0])
                {
                    throw new Exception("Unknown message format");
                }
                if (protocolMessage.Version != CreateOrUpdateObjectsMessage.Protocol.Version)
                {
                    throw new Exception("Message version mismatch");
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
