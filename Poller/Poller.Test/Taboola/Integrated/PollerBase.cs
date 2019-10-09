using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poller.Database;
using Poller.Taboola;
using Poller.Taboola.Traffic;
using System.Threading;

using AccountInternal = Maximiz.Model.Entity.Account;
using Poller.OAuth;
using Poller.Test.Taboola.Utility;
using System.Threading.Tasks;
using System.Linq;
using Abp.Threading;

namespace Poller.Test.Taboola.Integrated
{

    /// <summary>
    /// Contains all functionality we need to build poller test suites on top.
    /// Usage:
    /// - Inherit from this class;
    /// - Call <see cref="Setup"/> as test initializer;
    /// - Call <see cref="CleanUp"/> as test cleanup function;
    /// </summary>
    public class PollerBase
    {

        /// <summary>
        /// The poller object.
        /// </summary>
        internal TaboolaPoller _poller;

        /// <summary>
        /// External communication.
        /// </summary>
        internal CrudExternal _crudExternal;

        /// <summary>
        /// Internal communcation.
        /// </summary>
        internal CrudInternal _crudInternal;

        /// <summary>
        /// Account to which we commit our entities.
        /// </summary>
        internal AccountInternal _account;

        /// <summary>
        /// Options file importer and handler.
        /// </summary>
        internal TestConfigurationProvider _config;

        /// <summary>
        /// Contains our cancellation token.
        /// </summary>
        internal CancellationTokenSource _source;

        /// <summary>
        /// Utility functions for simulating an external entity creator (--> backend).
        /// </summary>
        internal CrudUtility _crudUtility;

        /// <summary>
        /// Used testing campaign name. This must be constant so that we can
        /// clean up what we have created no matter what happens.
        /// </summary>
        internal readonly string _campaignName = "Testing campaign name";

        /// <summary>
        /// Used ad item testing title. This must be constant so that we can
        /// remove all created entities as well.
        /// </summary>
        internal readonly string _adItemTitle = "Testing ad item title";

        /// <summary>
        /// Used to modify within the campaign.
        /// </summary>
        internal readonly string _updatedBrandingText = "Updated branding text";

        /// <summary>
        /// The URL where our ad items take us.
        /// </summary>
        internal readonly string _url = "http://www.laixer.com";

        /// <summary>
        /// This creates all required object to perform poller executions.
        /// </summary>
        internal void Setup()
        {
            // Token source
            _source = new CancellationTokenSource();

            // TODO Ontbeun
            _config = new TestConfigurationProvider(@"C:\Users\thoma\Programming\Laixer\MaxiMiz\Poller\Poller.Test\testconfig.json");
            var options = _config.GenerateTaboolaPollerOptions();

            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger("IntegratedCrudTest");
            var dbProvider = new DbProvider(_config.GetConnectionString("MaximizDatabase"));
            var cache = new MemoryCache(new MemoryCacheOptions());
            _poller = new TaboolaPoller(
                loggerFactory,
                options,
                dbProvider,
                cache);

            var _httpWrapper = new HttpWrapper(
                logger,
                new HttpManager(
                    new Uris(options.BaseUrl, "oauth/token", "oauth/token"),
                    new OAuthAuthorizationProvider
                    {
                        ClientId = options.OAuth2.ClientId,
                        ClientSecret = options.OAuth2.ClientSecret,
                        Username = options.OAuth2.Username,
                        Password = options.OAuth2.Password,
                    })
                );
            _crudExternal = new CrudExternal(logger, _httpWrapper);
            _crudInternal = new CrudInternal(logger, dbProvider, cache);
            _crudUtility = new CrudUtility(logger, _crudInternal, _crudExternal, dbProvider);

            // No accounts means we fail
            AsyncHelper.RunSync(() => ForceGetAccountsAsync());
            _account = (AsyncHelper.RunSync(() => _crudInternal.GetAdvertiserAccountsCachedAsync(_source.Token))).FirstOrDefault();
            Assert.IsNotNull(_account);
        }

        /// <summary>
        /// We must be sure to have accounts in our database, else the tests fail.
        /// </summary>
        private async Task ForceGetAccountsAsync()
        {
            var token = _source.Token;
            var accountsExternal = await _crudExternal.GetAllAccounts(token);
            await _crudInternal.CommitAccountBulk(accountsExternal, token);
        }

        /// <summary>
        /// Removes all the mess that we have created. This runs sync, even 
        /// though it contains async methods.
        /// </summary>
        internal void CleanUp()
        {
            _poller.Dispose();
            _source.Dispose();
        }

    }
}
