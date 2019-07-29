using Poller.EventBus;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionEventBusServiceExtensions
    {
        /// <summary>
        /// Add a DbProvider to the service container.
        /// </summary>
        /// <param name="dbConfigName">Database connection string.</param>
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
