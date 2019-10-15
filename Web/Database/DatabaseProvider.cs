using Laixer.Library.Injection.Database;
using Maximiz.Model.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Maximiz.Database
{

    /// <summary>
    /// Implementation for a database provider which can open database connections
    /// for us. This only calls the base constructor and configures project specific
    /// mapping.
    /// </summary>
    public class DatabaseProvider : DatabaseProviderNpgsql
    {
        /// <summary>
        /// Gets called once to ensure proper enum mapping.
        /// </summary>
        static DatabaseProvider()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<AdItemStatus>("ad_item_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ApprovalState>("approval_state");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<BidStrategy>("bid_strategy");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<BudgetModel>("budget_model");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<CampaignStatus>("campaign_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ConnectionType>("connection");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Location>("location");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Delivery>("delivery");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Device>("device");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<OS>("operating_system");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Publisher>("publisher");

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        /// <summary>
        /// Constructor to call base for dependency injection.
        /// </summary>
        /// <param name="configuration">The configuration file</param>
        /// <param name="options">The injected options</param>
        public DatabaseProvider(IConfiguration configuration, IOptionsMonitor<DatabaseProviderOptions> options)
            : base(configuration, options)
        {
        }
    }
}
