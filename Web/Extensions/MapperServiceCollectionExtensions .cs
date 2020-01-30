using Maximiz.Mapper;
using Maximiz.Model.Entity;
using Maximiz.ViewModels.EntityModels;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// External model to internal viewmodel mapper dependency injection.
    /// TODO Make more generic?
    /// </summary>
    public static class MapperServiceCollectionExtensions
    {

        /// <summary>
        /// Injects all required viewmodel mappers.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddSingleton<IMapper<CampaignWithStats, CampaignModel>, MapperCampaignWithStats>();
            services.AddSingleton<IMapper<AdGroupWithStats, AdGroupModel>, MapperAdGroupWithStats>();
            services.AddSingleton<IMapper<Account, AccountModel>, MapperAccount>();
            services.AddSingleton<IMapper<AccountWithStats, AccountModel>, MapperAccountWithStats>();
            // TODO Add other mappers too

            return services;
        }

    }
}