using Poller.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Extensions for service bus DI.
    /// </summary>
    public static class EventBusServiceCollectionExtensions
    {
        /// <summary>
        /// Add an event bus to the service container.
        /// </summary>
        /// <param name="dbConfigName">Event bus name</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddEventBusProvider(this IServiceCollection services, string dbConfigName)
        {
            services.AddSingleton<EventBusProvider>();
            services.Configure<EventBusProviderOptions>(options => options.ConnectionStringName = dbConfigName);

            return services;
        }
    }
}
