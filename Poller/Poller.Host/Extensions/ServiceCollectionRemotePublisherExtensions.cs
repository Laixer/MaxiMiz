using Microsoft.Extensions.DependencyInjection.Extensions;
using Poller.Publisher;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionRemotePublisherExtensions
    {
        /// <summary>
        /// Add an <see cref="IRemotePublisher"/> registration for the given type.
        /// </summary>
        /// <typeparam name="TRemotePublisher">An <see cref="IRemotePublisher"/> to register.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddRemotePublisher<TRemotePublisher>(this IServiceCollection services)
            where TRemotePublisher : class, IRemotePublisher
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IRemotePublisher, TRemotePublisher>());

            return services;
        }
    }
}
