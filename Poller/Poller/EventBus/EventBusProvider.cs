using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Poller.EventBus
{
    public class EventBusProvider
    {
        private readonly EventBusProviderOptions _options;
        private readonly string connectionString;

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="options">Configuration options.</param>
        public EventBusProvider(IConfiguration configuration, IOptions<EventBusProviderOptions> options)
        {
            Configuration = configuration;
            _options = options?.Value;
            connectionString = Configuration.GetConnectionString(_options.ConnectionStringName);
        }

        /// <summary>
        /// Create a new queue instance.
        /// </summary>
        /// <returns><see cref="IQueueClient"/> instance.</returns>
        public IQueueClient QueueClient(string queue) => new QueueClient(connectionString, queue);
    }
}
