using Laixer.Library.Injection;
using Laixer.Library.Injection.Database;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Database provider dependency injection extension.
    /// </summary>
    public static class DbProviderServiceCollectionExtensions
    {

        /// <summary>
        /// Add the database provider to the service container. This takes the
        /// <see cref="DatabaseProviderOptions"/> options file by default.
        /// </summary>
        /// <typeparam name="TDatabaseProvider">The database provider implementation</typeparam>
        /// <param name="connectionStringName">Database connection string name.
        /// This has to be present in the ConnectionStrings{} section of our
        /// <see cref="IConfiguration"/> file</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddDatabaseProvider<TDatabaseProvider>
            (this IServiceCollection services, string connectionStringName)
            where TDatabaseProvider : class, IDatabaseProvider
        {
            services.AddDatabaseProvider<TDatabaseProvider, DatabaseProviderOptions>(connectionStringName);
            return services;
        }

        /// <summary>
        /// Add the database provider to the service container.
        /// </summary>
        /// <typeparam name="TDatabaseProvider">The database provider implementation</typeparam>
        /// <typeparam name="TDatabaseProviderOptions">Options for provider</typeparam>
        /// <param name="connectionStringName">Database connection string name.
        /// This has to be present in the ConnectionStrings{} section of our
        /// <see cref="IConfiguration"/> file</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddDatabaseProvider<TDatabaseProvider, TDatabaseProviderOptions>
            (this IServiceCollection services, string connectionStringName)
            where TDatabaseProvider : class, IDatabaseProvider
            where TDatabaseProviderOptions : class, IOptionsBase
        {
            //services.AddSingleton<TDatabaseProviderOptions>();
            services.AddSingleton<IDatabaseProvider, TDatabaseProvider>();
            services.Configure<DatabaseProviderOptions>(options =>
                options.ConnectionStringName = connectionStringName);

            return services;
        }

    }
}