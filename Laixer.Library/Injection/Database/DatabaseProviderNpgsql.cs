using Laixer.Library.Configuration.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace Laixer.Library.Injection.Database
{

    /// <summary>
    /// Implementation for a database provider which can open database connections
    /// for us. In order to configure project specific postgres settings like 
    /// enum mapping, create a static constructor and call all configurations in
    /// there.
    /// </summary>
    public class DatabaseProviderNpgsql : IDatabaseProvider
    {

        /// <summary>
        /// Options file for this database provider.
        /// </summary>
        private readonly DatabaseProviderOptions _options;

        /// <summary>
        /// Contains our connection string.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Constructor to setup our database provider configuration through DI.
        /// </summary>
        /// <param name="configuration">The configuration file</param>
        /// <param name="connectionStringName">The name of the connection string</param>
        public DatabaseProviderNpgsql(IConfiguration configuration, IOptionsMonitor<DatabaseProviderOptions> options)
        {
            _options = options.CurrentValue;
            _connectionString = configuration.GetConnectionString(_options.ConnectionStringName);

            // Throw if we can't connect to the database for quick debug
            if (_connectionString == null)
            {
                throw new ConfigurationException("Missing database connection string");
            }
        }

        /// <summary>
        /// Creates a new connection to the database.
        /// </summary>
        /// <returns>The new connection</returns>
        public IDbConnection GetConnectionScope() => new NpgsqlConnection(_connectionString);

    }
}
