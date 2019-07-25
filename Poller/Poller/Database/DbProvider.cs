﻿using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Poller.Database
{
    public class DbProvider
    {
        private readonly DbProviderOptions _options;
        private readonly string connectionString;

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        public DbProvider(IConfiguration configuration, IOptions<DbProviderOptions> options)
        {
            Configuration = configuration;
            _options = options?.Value;
            connectionString = Configuration.GetConnectionString(_options.ConnectionStringName);
        }

        /// <summary>
        /// Create a new connection instance.
        /// </summary>
        /// <returns><see cref="IDbConnection"/> instance.</returns>
        public IDbConnection ConnectionScope() => new NpgsqlConnection(connectionString);
    }
}