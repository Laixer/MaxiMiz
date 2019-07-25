using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Poller.Database
{
    public class DbProvider
    {
        private DbProviderOptions _options;

        public IConfiguration Configuration { get; }

        public DbProvider(IConfiguration configuration, IOptions<DbProviderOptions> options)
        {
            Configuration = configuration;
            _options = options?.Value;
        }

        public IDbConnection ConnectionScope()
        {
            return new NpgsqlConnection(Configuration.GetConnectionString(_options.ConnectionStringName));
        }
    }
}
