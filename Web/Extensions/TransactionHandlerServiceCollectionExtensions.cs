using Maximiz.Model.Entity;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Transaction handler dependency injection extension.
    /// </summary>
    public static class TransactionHandlerServiceCollectionExtensions
    {

        /// <summary>
        /// Adds a transaction handler to the service collection.
        /// </summary>
        /// <typeparam name="TTransactionHandler">The transaction handler type</typeparam>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection</returns>
        //public static IServiceCollection AddTransactionHandler
        //    <TTransactionHandler>(this IServiceCollection services)
        //    where TTransactionHandler : class, ITransactionHandler
        //{
        //    // First add creation dependencies
        //    services.AddSingleton<ICud<Campaign>, CudCampaign>();
        //    services.AddSingleton<ICud<CampaignGroup>, CudCampaignGroup>();
        //    services.AddSingleton<ICud<AdItem>, CudAdItem>();
        //    services.AddSingleton<ICud<AdGroup>, CudAdGroup>();
        //    services.AddSingleton<ICudProcessor, CudProcessor>();

        //    // Then add sending dependencies
        //    services.AddSingleton<ISender<Entity>, Sender>();

        //    // Then add the transaction handler
        //    services.AddSingleton<ITransactionHandler, TTransactionHandler>();

        //    // Return services for chaining
        //    return services;
        //}

    }
}