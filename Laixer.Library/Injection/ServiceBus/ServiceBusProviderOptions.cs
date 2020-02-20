using Microsoft.Extensions.Configuration;

namespace Laixer.Library.Injection.ServiceBus
{

    /// <summary>
    /// Options file for a <see cref="ServiceBusProvider"/> implementation.
    /// </summary>
    public class ServiceBusProviderOptions : IOptionsBase
    {

        /// <summary>
        /// The name of the connection string in the ConnectionStrings{} section
        /// of our <see cref="IConfiguration"/> file.
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// Contains the name of the used queue.
        /// </summary>
        public string QueueName { get; set; }

    }
}
