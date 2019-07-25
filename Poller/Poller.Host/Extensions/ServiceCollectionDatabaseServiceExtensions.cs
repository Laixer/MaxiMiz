using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Poller.Database;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionDatabaseServiceExtensions
    {
        /// <summary>
        /// Add a DbProvider to the service container.
        /// </summary>
        /// <param name="dbConfigName">Database connection string.</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddNpgsql(this IServiceCollection services, string dbConfigName)
        {
            services.AddSingleton<DbProvider>();
            services.Configure<DbProviderOptions>(options => options.ConnectionStringName = dbConfigName);

            return services;
        }
    }
}
