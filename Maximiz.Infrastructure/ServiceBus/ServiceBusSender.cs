using Laixer.AppSettingsValidation.Exceptions;
using Maximiz.Core.Infrastructure.EventQueue;
using Maximiz.Core.Utility;
using Maximiz.Model.Protocol;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.ServiceBus
{

    /// <summary>
    /// Sends one or multiple messages to an Azure Service Bus.
    /// Implements <see cref="IEventQueueSender"/>.
    /// </summary>
    public sealed class ServiceBusSender : IEventQueueSender
    {

        private readonly string connectionString;
        private readonly string queueName;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public ServiceBusSender(IOptions<ServiceBusSenderOptions> options,
            IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            if (configuration == null) { throw new ConfigurationException(nameof(configuration)); }

            if (options.Value == null) { throw new ArgumentNullException(nameof(options)); }
            if (string.IsNullOrEmpty(options.Value.ConnectionStringName)) { throw new ArgumentNullException(nameof(options.Value.ConnectionStringName)); }
            if (string.IsNullOrEmpty(options.Value.QueueName)) { throw new ArgumentNullException(nameof(options.Value.QueueName)); }

            var section = configuration.GetSection(options.Value.QueueName);
            if (section == null || string.IsNullOrEmpty(section.Value)) { throw new ConfigurationException($"Configuration does not contain the service bus queue name: {options.Value.QueueName}"); }
            queueName = section.Value;

            connectionString = configuration.GetConnectionString(options.Value.ConnectionStringName);
            if (string.IsNullOrEmpty(connectionString)) { throw new ConfigurationException($"Configuration does not contains connection string with name {options.Value.ConnectionStringName}"); }

            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            logger = loggerFactory.CreateLogger(nameof(ServiceBusSender));
        }

        /// <summary>
        /// Sends a <see cref="OperationMessage"/> to an azure service bus.
        /// </summary>
        /// <remarks>
        /// This will throw an <see cref="InvalidOperationException"/> if we 
        /// fail somewhere down the line.
        /// </remarks>
        /// <param name="message"><see cref="OperationMessage"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task SendMessageAsync(OperationMessage message, CancellationToken token)
        {
            MessageValidator.Validate(message);
            if (token == null) { throw new ArgumentNullException(nameof(token)); }

            try
            {
                var client = GetQueueClient();
                var formatter = new BinaryFormatter();
                using (var stream = new MemoryStream())
                {
                    formatter.Serialize(stream, message);
                    var toSend = new Message(stream.ToArray());
                    await client.SendAsync(toSend);
                    logger.LogTrace($"Message was sent to service bus successfully for operation {message.OperationId}");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error while sending object to service bus for operation {message.OperationId}");
                throw new InvalidOperationException($"Could not send message to service bus for operation {message.OperationId} ", e);
            }
        }

        /// <summary>
        /// Sends all messages in a collection of <see cref="OperationMessage"/>s.
        /// </summary>
        /// <param name="messages"><see cref="IEnumerable{OperationMessage}"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        public Task SendMessagesAsync(IEnumerable<OperationMessage> messages, CancellationToken token)
        {
            if (token == null) { throw new ArgumentNullException(nameof(token)); }

            // TODO Smart parallelization
            // TODO ATOM
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new <see cref="QueueClient"/> to communicatie with our
        /// service bus.
        /// </summary>
        /// <returns><see cref="QueueClient"/></returns>
        private QueueClient GetQueueClient()
            => new QueueClient(connectionString, queueName);

    }
}
