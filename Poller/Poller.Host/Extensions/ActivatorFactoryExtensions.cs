using Poller.Scheduler.Activator;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ActivatorFactoryExtensions
    {
        /// <summary>
        /// Add activator factory to the service container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddActivatorFactory(this IServiceCollection services)
        {
            services.AddSingleton<ActivatorFactory>();

            return services;
        }
    }
}
