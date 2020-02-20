using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Poller.Database;
using Poller.Model;
using Poller.Poller;
using Poller.Taboola;
using System;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Contains extensions for poller DI.
    /// </summary>
    public static class PollerServiceCollectionExtensions
    {

        /// <summary>
        /// Adds the taboola poller to the service collection for DI.
        /// TODO Clean up with builder
        /// TODO Make options generic
        /// </summary>
        /// <typeparam name="TPoller">The poller interface</typeparam>
        /// <typeparam name="TPollerOptions">The poller options</typeparam>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddPoller<TPoller, TPollerOptions>
            (this IServiceCollection services)
            where TPoller : class, IPoller
            where TPollerOptions : class
        {
            // Add poller
            services.AddSingleton((provider) =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var dbProvider = provider.GetService<DbProvider>();

                var options = provider.GetService<TaboolaPollerOptions>();

                var logger = provider.GetService<ILoggerFactory>();
                var cache = provider.GetService<IMemoryCache>();

                return new TaboolaPoller(logger, options, dbProvider, cache);
            });

            return services;
        }

        /// <summary>
        /// Adds the Taboola Poller options to the service collection for DI.
        /// TODO Clean up with builder
        /// TODO Incorrect options pattern usage. This is beun but bind section doesnt work.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddTaboolaPollerOptions(this IServiceCollection services)
        {
            services.AddSingleton((configuration) =>
            {
                var config = configuration.GetRequiredService<IConfiguration>();
                var result = new TaboolaPollerOptions
                {
                    BaseUrl = config.GetValue<string>("Taboola_BaseUrl"),
                    OAuth2 = new OAuth2
                    {
                        ClientId = config.GetValue<string>("Taboola_OAuth2_ClientId"),
                        ClientSecret = config.GetValue<string>("Taboola_OAuth2_ClientSecret"),
                        Username = config.GetValue<string>("Taboola_OAuth2_Username"),
                        Password = config.GetValue<string>("Taboola_OAuth2_Password"),
                        GrantType = config.GetValue<string>("Taboola_OAuth2_GrantType")
                    },
                    CreateOrUpdateObjectsEventBus = config.GetValue<string>("ServiceBusQueueName")
                };
                return result;
            });

            return services;
        }

    }
}
