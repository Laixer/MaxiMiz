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
        public static IServiceCollection AdddViewModelServices(this IServiceCollection services)
        {
            // Add view model services
            services.AddSingleton<ICurrencyViewModelService, CurrencyViewModelService>();
            services.AddSingleton<IEnumViewModelService, EnumViewModelService>();
            services.AddSingleton<ITargetingViewModelService, TargetingViewModelService>();

            // Return for chaining
            return services;
        }

    }
}