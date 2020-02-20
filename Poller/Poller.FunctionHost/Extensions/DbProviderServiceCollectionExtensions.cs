using Microsoft.Extensions.DependencyInjection;
using Poller.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Extensions for DI for database.
    /// </summary>
    public static class DbProviderServiceCollectionExtensions
    {
        /// <summary>
        /// Add a DbProvider to the service container.
        /// </summary>
        /// <param name="dbConfigName">Database connection string.</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddDbProvider(this IServiceCollection services, string dbConfigName)
        {
            services.AddSingleton<DbProvider>();
            services.Configure<DbProviderOptions>(options => options.ConnectionStringName = dbConfigName);

            return services;
        }

    }
}
