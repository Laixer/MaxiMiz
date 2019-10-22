using Maximiz.Database;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Crud internal dependency injection extension.
    /// TODO Generic cleanup.
    /// </summary>
    public static class CrudInternalServiceCollectionExtensions
    {

        /// <summary>
        /// Adds the crud internal to the service collection.
        /// </summary>
        /// <typeparam name="TCrudInternal">Crud internal implementation</typeparam>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        //public static IServiceCollection AddCrudInternal<TICrudInternal, TCrudInternal>(this IServiceCollection services)
        //    where TICrudInternal : ICrudInternal
        //    where TCrudInternal : class, TICrudInternal
        //{
        //    services.AddSingleton<TICrudInternal, TCrudInternal>();
        //    return services;
        //}

        /// <summary>
        /// Adds the crud internal to the service collection.
        /// </summary>
        /// <typeparam name="TCrudInternal">Crud internal implementation</typeparam>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddCrudInternal<TCrudInternal>(this IServiceCollection services)
            where TCrudInternal : class, ICrudInternal
        {
            services.AddSingleton<ICrudInternal, TCrudInternal>();
            return services;
        }

        /// <summary>
        /// Adds the crud internal to the service collection.
        /// </summary>
        /// <typeparam name="TCrudInternal">Crud internal implementation</typeparam>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddCrudInternalWebClient<TCrudInternal>(this IServiceCollection services)
            where TCrudInternal : class, ICrudInternalWebClient
        {
            services.AddSingleton<ICrudInternalWebClient, TCrudInternal>();
            return services;
        }
    }
}