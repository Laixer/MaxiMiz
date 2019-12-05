using Laixer.AppSettingsValidation.Exceptions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;

namespace Maximiz.Infrastructure.Database
{

    /// <summary>
    /// Provides connection scopes to a postgres database.
    /// </summary>
    public sealed class NpgsqlDatabaseProvider : IDatabaseProvider
    {

        private readonly string connectionString;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public NpgsqlDatabaseProvider(NpgsqlDatabaseProviderOptions options,
            IConfiguration configuration)
        {
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }
            if (options == null) { throw new ArgumentNullException(nameof(options)); }
            if (string.IsNullOrEmpty(options.ConnectionStringName)) { throw new ConfigurationException(nameof(options.ConnectionStringName)); }

            connectionString = configuration.GetConnectionString(options.ConnectionStringName);
            if (string.IsNullOrEmpty(connectionString)) { throw new ConfigurationException($"IConfiguration does not contains connection string with name {options.ConnectionStringName}"); }
        }

        /// <summary>
        /// Gets a connection scope.
        /// </summary>
        /// <returns><see cref=""/></returns>
        public IDbConnection GetConnectionScope()
            => new NpgsqlConnection(connectionString);

    }
}
