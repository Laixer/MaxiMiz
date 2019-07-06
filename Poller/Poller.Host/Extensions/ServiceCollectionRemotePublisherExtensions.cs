using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
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

        /// <summary>
        /// Add an <see cref="IRemotePublisher"/> registration for the given type.
        /// </summary>
        /// <typeparam name="TRemotePublisher">An <see cref="IRemotePublisher"/> to register.</typeparam>
        /// <typeparam name="TRemotePublisherOptions">Options for <see cref="IRemotePublisher"/>.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddRemotePublisher<TRemotePublisher, TRemotePublisherOptions>(this IServiceCollection services)
            where TRemotePublisher : class, IRemotePublisher
            where TRemotePublisherOptions : class
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IRemotePublisher, TRemotePublisher>());
            services.Configure<TRemotePublisherOptions>(configuration);

            var annotatedPublisher = typeof(TRemotePublisher).GetCustomAttributes(typeof(PublisherAttribute), false);
            foreach (PublisherAttribute item in annotatedPublisher)
            {
                services.Configure<TRemotePublisherOptions>(string.IsNullOrEmpty(item.Name)
                    ? configuration
                    : configuration.GetSection(item.Name));
            }

            return services;
        }
    }
}
