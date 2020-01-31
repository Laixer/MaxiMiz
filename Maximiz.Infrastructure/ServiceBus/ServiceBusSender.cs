using Laixer.AppSettingsValidation.Exceptions;
using Maximiz.Core.Infrastructure.EventQueue;
using Maximiz.Model.Protocol;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.ServiceBus
{

    /// <summary>
    /// Sends one or multiple messages to an Azure Service Bus.
    /// Implements <see cref="IEventQueueSender"/>.
    /// </summary>
    public sealed class ServiceBusSender : IEventQueueSender
    {

        private readonly ServiceBusSenderOptions _options;
        private readonly string connectionString;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public ServiceBusSender(ServiceBusSenderOptions options, 
            IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            if (configuration == null) { throw new ConfigurationException(nameof(configuration)); }

            if (options == null) { throw new ArgumentNullException(nameof(options)); }
            if (string.IsNullOrEmpty(options.ConnectionStringName)) { throw new ConfigurationException(nameof(options.ConnectionStringName)); }
            if (string.IsNullOrEmpty(options.QueueName)) { throw new ConfigurationException(nameof(options.QueueName)); }
            _options = options;

            connectionString = configuration.GetConnectionString(_options.ConnectionStringName);
            if (string.IsNullOrEmpty(connectionString)) { throw new ConfigurationException($"IConfiguration does not contains connection string with name {_options.ConnectionStringName}"); }

            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            logger = loggerFactory.CreateLogger(nameof(ServiceBusSender));
        }

        /// <summary>
        /// Sends a <see cref="CreateOrUpdateObjectsMessage"/> to an azure service bus.
        /// </summary>
        /// <remarks>
        /// On error this will catch the exception, log it and return false.
        /// </remarks>
        /// <param name="message"><see cref="CreateOrUpdateObjectsMessage"/></param>
        /// <returns>True if successful, false if not</returns>
        public async Task<bool> SendMessageAsync(CreateOrUpdateObjectsMessage message)
        {
            if (message == null) { throw new ArgumentNullException(nameof(message)); }

            try
            {
                var client = GetQueueClient();
                var formatter = new BinaryFormatter();
                using (var stream = new MemoryStream())
                {
                    formatter.Serialize(stream, message);
                    var toSend = new Message(stream.ToArray());
                    await client.SendAsync(toSend);
                    logger.LogTrace("Sent message to service bus successfully");
                }
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error while sending object to service bus");
                return false;
            }
        }

        /// <summary>
        /// Sends all messages in a collection of <see cref="CreateOrUpdateObjectsMessage"/>s.
        /// </summary>
        /// <remarks>
        /// This will not stop when one of the messages can't be sent.
        /// </remarks>
        /// <param name="messages"><see cref="IEnumerable{CreateOrUpdateObjectsMessage}"/></param>
        /// <returns>True if successful, false if not</returns>
        public async Task<bool> SendMessagesAsync(IEnumerable<CreateOrUpdateObjectsMessage> messages)
        {
            if (messages == null) { throw new ArgumentNullException(nameof(messages)); }

            // TODO Smart parallelization
            var result = true;
            foreach (var message in messages)
            {
                if (await SendMessageAsync(message) == false)
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a new <see cref="QueueClient"/> to communicatie with our
        /// service bus.
        /// </summary>
        /// <returns><see cref="QueueClient"/></returns>
        private QueueClient GetQueueClient()
            => new QueueClient(connectionString, _options.QueueName);

    }
}
