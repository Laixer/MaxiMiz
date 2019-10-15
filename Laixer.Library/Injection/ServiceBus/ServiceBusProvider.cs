using Laixer.Library.Configuration.Exceptions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Laixer.Library.Injection.ServiceBus
{

    /// <summary>
    /// An injectable service bus connection provider.
    /// </summary>
    public class ServiceBusProvider : IServiceBusProvider
    {

        /// <summary>
        /// Options file for this service bus provider.
        /// </summary>
        private readonly ServiceBusProviderOptions _options;

        /// <summary>
        /// Contains our connection string.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <param name="options">Options for this object</param>
        public ServiceBusProvider(IConfiguration configuration, IOptionsMonitor<ServiceBusProviderOptions> options)
        {
            _options = options.CurrentValue;
            _connectionString = configuration.GetConnectionString(_options.ConnectionStringName);

            // Throw if we can't connect to the service bus for quick debug
            if (_connectionString == null) { throw new ConfigurationException("Missing service bus connection string"); }
            if (_options.QueueName == null) { throw new ConfigurationException("Missing service bus queue name"); }
        }

        /// <summary>
        /// Creates a new queue client.
        /// </summary>
        /// <returns>The new queue client</returns>
        public QueueClient GetQueueClient()
        {
            return new QueueClient(_connectionString, _options.QueueName);
        }

    }
}
