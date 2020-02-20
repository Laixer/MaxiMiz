using Maximiz.Services;
using Maximiz.Services.Abstraction;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// External model to internal viewmodel mapper dependency injection.
    /// TODO Make more generic?
    /// </summary>
    public static class ViewModelServiceServiceCollectionExtensions
    {

        /// <summary>
        /// Injects all required viewmodel mappers.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddViewModelServices(this IServiceCollection services)
        {
            services.AddTransient<ICurrencyViewModelService, CurrencyViewModelService>();
            services.AddTransient<IEnumViewModelService, EnumViewModelService>();
            services.AddTransient<ITargetingViewModelService, TargetingViewModelService>();
            services.AddTransient<IUserViewModelService, UserViewModelService>();

            // Return for chaining
            return services;
        }

    }
}