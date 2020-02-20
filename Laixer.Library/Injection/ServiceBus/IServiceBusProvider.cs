using Microsoft.Azure.ServiceBus;

namespace Laixer.Library.Injection.ServiceBus
{

    /// <summary>
    /// Interface to implement a service bus queue client provider.
    /// </summary>
    public interface IServiceBusProvider
    {

        /// <summary>
        /// Retrieves our queue client.
        /// </summary>
        /// <returns></returns>
        QueueClient GetQueueClient();

    }
}
