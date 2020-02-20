using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Infrastructure.Database;
using Maximiz.Infrastructure.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Contains functionality to setup dependency injection for all our
    /// <see cref="IRepository"/> implementations at once.
    /// </summary>
    public static class RepositoryServiceCollectionExtensions
    {

        /// <summary>
        /// Injects all our <see cref="IRepository{TEntity}"/> implementations.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // TODO Check which we don't need, clean up

            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IAccountWithStatsRepository, AccountWithStatsRepository>();
            services.AddTransient<IAdGroupRepository, AdGroupRepository>();
            services.AddTransient<IAdGroupWithStatsRepository, AdGroupWithStatsRepository>();
            services.AddTransient<IAdItemRepository, AdItemRepository>();
            services.AddTransient<IAdItemWithStatsRepository, AdItemWithStatsRepository>();
            services.AddTransient<ICampaignGroupRepository, CampaignGroupRepository>();
            services.AddTransient<ICampaignGroupWithStatsRepository, CampaignGroupWithStatsRepository>();
            
            services.AddTransient<ICampaignRepository, CampaignRepository>(); // TODO Disable
            
            services.AddTransient<ICampaignWithStatsRepository, CampaignWithStatsRepository>();
            services.AddTransient<IOperationRepository, OperationRepository>();

            return services;
        }

        /// <summary>
        /// Configures our <see cref="IDatabaseProvider"/>.
        /// 
        /// TODO Move this
        /// 
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="connectionStringName">Name of the database connection 
        /// string as specified in the appsettings</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddDatabaseProvider(this IServiceCollection services, string connectionStringName)
        {
            services.AddTransient<IDatabaseProvider, NpgsqlDatabaseProvider>();
            services.Configure<NpgsqlDatabaseProviderOptions>(options =>
            {
                options.ConnectionStringName = connectionStringName;
            });

            return services;
        }

    }

}
