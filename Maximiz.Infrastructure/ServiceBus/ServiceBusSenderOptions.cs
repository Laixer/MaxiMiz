
namespace Maximiz.Infrastructure.ServiceBus
{

    /// <summary>
    /// Contains the options for our <see cref="ServiceBusSender"/>.
    /// </summary>
    public sealed class ServiceBusSenderOptions
    {

        /// <summary>
        /// Name of the connection string in the IConfiguration file.
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// Name of the used queue within the service bus.
        /// </summary>
        public string QueueName { get; set; }

    }
}
