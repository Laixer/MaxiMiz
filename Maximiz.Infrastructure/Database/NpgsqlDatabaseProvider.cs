using Laixer.AppSettingsValidation.Exceptions;
using Maximiz.Model.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using Npgsql.Logging;
using System;
using System.Data;

namespace Maximiz.Infrastructure.Database
{

    /// <summary>
    /// Provides connection scopes to a postgres database.
    /// </summary>
    public sealed class NpgsqlDatabaseProvider : IDatabaseProvider
    {
        private static bool setLogger = false;
        private readonly string connectionString;

        /// <summary>
        /// Setup enum mapping once.
        /// </summary>
        static NpgsqlDatabaseProvider()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<AdItemStatus>("ad_item_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ApprovalState>("approval_state");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<BidStrategy>("bid_strategy");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<BudgetModel>("budget_model");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<CampaignStatus>("campaign_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ConnectionType>("connection");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Delivery>("delivery");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Device>("device");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Location>("location");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<OS>("operating_system");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<OperationItemStatus>("operation_item_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Publisher>("publisher");

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        /// <summary>
        /// Constructor for dependency injection.
        /// TODO Clean up a bit
        /// </summary>
        public NpgsqlDatabaseProvider(IOptions<NpgsqlDatabaseProviderOptions> options,
            IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            // TODO Logging for now
            if (!setLogger)
            {
                NpgsqlLogManager.Provider = new NpgsqlLoggingProvider(loggerFactory);
                setLogger = true;
            }

            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }
            if (options.Value == null) { throw new ArgumentNullException(nameof(options.Value)); }
            if (string.IsNullOrEmpty(options.Value.ConnectionStringName)) { throw new ConfigurationException(nameof(options.Value.ConnectionStringName)); }

            connectionString = configuration.GetConnectionString(options.Value.ConnectionStringName);
            if (string.IsNullOrEmpty(connectionString)) { throw new ConfigurationException($"IConfiguration does not contains connection string with name {options.Value.ConnectionStringName}"); }

        }

        /// <summary>
        /// Gets a connection scope.
        /// </summary>
        /// <returns><see cref=""/></returns>
        public IDbConnection GetConnectionScope() => new NpgsqlConnection(connectionString);

    }
}
