using Laixer.Library.Injection;
using Laixer.Library.Injection.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Database provider dependency injection extension.
    /// </summary>
    public static class ServiceBusServiceCollectionExtensions
    {

        /// <summary>
        /// Add a service bus provider to the service container. This takes the
        /// <see cref="ServiceBusProviderOptions"/> options file by default.
        /// 
        /// TODO config injection is ugly
        /// </summary>
        /// <typeparam name="TServiceBusProvider">The service bus provider implementation</typeparam>
        /// <param name="connectionStringName">Service bus connection string name.
        /// This has to be present in the ConnectionStrings{} section of our
        /// <see cref="IConfiguration"/> file</param>
        /// <param name="queueNameInConfig">The name of the variable in our
        /// <see cref="IConfiguration"/> file that holds the queue name</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <param name="configuration">The configuration file</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddServiceBusProvider<TServiceBusProvider>
            (this IServiceCollection services, string connectionStringName, string queueNameInConfig, IConfiguration configuration)
            where TServiceBusProvider : class, IServiceBusProvider
        {
            services.AddServiceBusProvider<TServiceBusProvider, ServiceBusProviderOptions>(
                connectionStringName, queueNameInConfig, configuration);
            return services;
        }

        /// <summary>
        /// Add a service bus provider to the service container.
        /// 
        /// TODO config injection is ugly
        /// </summary>
        /// <typeparam name="TServiceBusProvider">The service bus provider implementation</typeparam>
        /// <param name="connectionStringName">Service bus connection string name.
        /// This has to be present in the ConnectionStrings{} section of our
        /// <see cref="IConfiguration"/> file</param>
        /// <param name="queueNameInConfig">The name of the variable in our
        /// <see cref="IConfiguration"/> file that holds the queue name</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <param name="configuration">The configuration file</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddServiceBusProvider<TServiceBusProvider, TServiceBusProviderOptions>
            (this IServiceCollection services, string connectionStringName, string queueNameInConfig, IConfiguration configuration)
            where TServiceBusProvider : class, IServiceBusProvider
            where TServiceBusProviderOptions : class, IOptionsBase
        {
            // TODO Skip if already present?
            services.AddSingleton<TServiceBusProviderOptions>();
            services.AddSingleton<IServiceBusProvider, TServiceBusProvider>();
            services.Configure<ServiceBusProviderOptions>(options =>
            {
                options.ConnectionStringName = connectionStringName;
                options.QueueName = configuration.GetValue<string>(queueNameInConfig);
            });

            return services;
        }

        /// <summary>
        /// Injects a service bus sender object to the service collection. There
        /// is no need to explicitly add the <typeparamref name="TServiceBusProvider"/>
        /// object to the service collections, as this function handles that
        /// internally. This takes the <see cref="ServiceBusProviderOptions"/>
        /// options file by default
        /// 
        /// TODO Clean up IConfiguration injection, it's ugly
        /// </summary>
        /// <typeparam name="TServiceBusSender">The service bus sender type</typeparam>
        /// <typeparam name="TServiceBusProvider">The service bus provider type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="connectionStringName">The name of the service bus connection string variable</param>
        /// <param name="queueNameInConfig">The name of the service bus queue name variable</param>
        /// <param name="configuration">The configuration</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddServiceBusSender<TServiceBusSender, TServiceBusProvider>
            (this IServiceCollection services, string connectionStringName,
            string queueNameInConfig, IConfiguration configuration)
            where TServiceBusProvider : class, IServiceBusProvider
            where TServiceBusSender : class, IServiceBusSender
        {
            // First add the service bus provider
            services.AddServiceBusProvider<TServiceBusProvider>
                (connectionStringName, queueNameInConfig, configuration);

            // Then add the service bus sender
            services.AddSingleton<IServiceBusSender, TServiceBusSender>();

            // Return services object to allow chaining
            return services;
        }

        /// <summary>
        /// Injects a service bus sender object to the service collection. There
        /// is no need to explicitly add the <typeparamref name="TServiceBusProvider"/>
        /// object to the service collections, as this function handles that
        /// internally.
        /// 
        /// TODO Clean up IConfiguration injection, it's ugly
        /// </summary>
        /// <typeparam name="TServiceBusSender">The service bus sender type</typeparam>
        /// <typeparam name="TServiceBusProvider">The service bus provider type</typeparam>
        /// <typeparam name="TServiceBusProviderOptions">Options for service bus provider</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="connectionStringName">The name of the service bus connection string variable</param>
        /// <param name="queueNameInConfig">The name of the service bus queue name variable</param>
        /// <param name="configuration">The configuration</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddServiceBusSender<TServiceBusSender, TServiceBusProvider, TServiceBusProviderOptions>
            (this IServiceCollection services, string connectionStringName,
            string queueNameInConfig, IConfiguration configuration)
            where TServiceBusProvider : class, IServiceBusProvider
            where TServiceBusSender : class, IServiceBusSender
            where TServiceBusProviderOptions : class, IOptionsBase
        {
            // First add the service bus provider
            services.AddServiceBusProvider<TServiceBusProvider, TServiceBusProviderOptions>
                (connectionStringName, queueNameInConfig, configuration);

            // Then add the service bus sender
            services.AddSingleton<IServiceBusSender, TServiceBusSender>();

            // Return services object to allow chaining
            return services;
        }

    }
}