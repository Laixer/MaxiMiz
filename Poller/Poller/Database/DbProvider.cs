using System.Data;
using Maximiz.Model.Enums;
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
        /// Static instance of the database provider.
        /// </summary>
        static DbProvider()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

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
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Maximiz.Model.Enums.Publisher>("publisher");
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="options">Configuration options.</param>
        public DbProvider(IConfiguration configuration, IOptions<DbProviderOptions> options)
        {
            Configuration = configuration;
            _options = options?.Value;
            connectionString = Configuration.GetConnectionString(_options.ConnectionStringName);
        }


        /// <summary>
        /// Constructor for testing purposes.
        /// TODO Might want to remove this.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        public DbProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Create a new connection instance.
        /// </summary>
        /// <returns><see cref="IDbConnection"/> instance.</returns>
        public IDbConnection ConnectionScope()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
