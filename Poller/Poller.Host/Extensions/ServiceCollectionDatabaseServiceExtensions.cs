using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionDatabaseServiceExtensions
    {
        /// <summary>
        /// Add a Npgsql to the service container.
        /// </summary>
        /// <param name="dbConfigName">Database connection string.</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddNpgsql(this IServiceCollection services, string dbConfigName)
        {
            services.AddSingleton<DbConnection>((provider) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                return new NpgsqlConnection(configuration.GetConnectionString(dbConfigName));
            });

            return services;
        }
    }
}
