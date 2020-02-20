using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Infrastructure.Committing;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Extensions for 
    /// </summary>
    public static class CommitterServiceCollectionExtensions
    {

        /// <summary>
        /// Adds all <see cref="ICommitter{TEntity}"/>s to the dependency injector. 
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddCommitters(this IServiceCollection services)
        {
            // TODO Implement
            services.AddTransient<ILinkingCommitter, LinkingCommitter>();
            services.AddTransient<IOperationItemCommitter, OperationItemCommitter>();

            services.AddSingleton<IAccountCommitter, AccountCommitter>();
            services.AddTransient<ICampaignCommitter, CampaignCommitter>();
            services.AddTransient<ICampaignGroupCommitter, CampaignGroupCommitter>();
            services.AddTransient<IAdItemCommitter, AdItemCommitter>();
            services.AddTransient<IAdGroupCommitter, AdGroupCommitter>();

            return services;
        }

    }
}
