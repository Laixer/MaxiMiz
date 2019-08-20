using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Poller.Database;
using Poller.Extensions;
using Poller.OAuth;
using Poller.Poller;

using AccountEntity = Maximiz.Model.Entity.Account;
using CampaignEntity = Maximiz.Model.Entity.Campaign;
using AdItemEntity = Maximiz.Model.Entity.AdItem;
using Poller.Taboola.Mapper;

namespace Poller.Taboola
{

    /// <summary>
    /// Partial class for our Taboola Poller. This
    /// part implements all required activator base
    /// interfaces.
    /// </summary>
    internal partial class TaboolaPoller : IPollerRefreshAdvertisementData, IPollerDataSyncback, IPollerCreateOrUpdateObjects, IDisposable
    {
        private readonly ILogger _logger;
        private readonly DbProvider _provider;
        private readonly IMemoryCache _cache;
        private readonly HttpManager _client;

        private readonly MapperAccount _mapperAccount;
        private readonly MapperCampaign _mapperCampaign;
        private readonly MapperAdItem _mapperAdItem;

        /// <summary>
        /// Constructor with dependency injection.
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="options">The options</param>
        /// <param name="provider">The database provider</param>
        /// <param name="cache">The cache</param>
        public TaboolaPoller(ILoggerFactory logger, TaboolaPollerOptions options, DbProvider provider, IMemoryCache cache)
        {
            _logger = logger.CreateLogger(typeof(TaboolaPoller).FullName);
            _provider = provider;
            _cache = cache;

            _client = new HttpManager(options.BaseUrl)
            {
                TokenUri = "oauth/token",
                RefreshUri = "oauth/token",
                AuthorizationProvider = new OAuthAuthorizationProvider
                {
                    ClientId = options.OAuth2.ClientId,
                    ClientSecret = options.OAuth2.ClientSecret,
                    Username = options.OAuth2.Username,
                    Password = options.OAuth2.Password,
                }
            };

            _mapperAccount = new MapperAccount();
            _mapperCampaign = new MapperCampaign();
            _mapperAdItem = new MapperAdItem();

            _logger.LogInformation("Taboola poller created");
        }

        /// <summary>
        /// Implementation from our data refresh interface.
        /// This function calls the Taboola API and retrieves
        /// our accounts, campaigns and ad items.
        /// </summary>
        /// <param name="context">The poller context</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Nothing (task)</returns>
        public async Task RefreshAdvertisementDataAsync(
            PollerContext context, CancellationToken token)
        {
            var accounts = await FetchLocalAdvertiserAccountsForCache(token);

            foreach (var account in accounts.ToList().Shuffle())
            {
                var result = await GetTopCampaignReportAsync(
                    account.Name, token);
                var converted = _mapperAdItem.ConvertAll(result.Items);

                await CommitCampaignItemsConverted(converted, token);

                // Prevent spamming our API
                await Task.Delay(250, token);
            }
        }

        /// <summary>
        /// Implements our data syncback interface. This is used
        /// to check if any values have been changed in Taboola's
        /// own interface. With this we will always remain up to
        /// date.
        /// </summary>
        /// <param name="context">The poller context</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Nothing (task)</returns>
        public async Task DataSyncbackAsync(PollerContext context, CancellationToken token)
        {
            // Accounts almost never change, only do this once every 4 iterations.
            // TODO Update account details when they are changed
            if (context.RunCount + 1 % 4 == 0)
            {
                _logger.LogInformation("Syncback account information");

                var result = await GetAllAccounts(token);

                await CommitAccounts(result, token);
            }

            var accounts = await FetchAdvertiserAccountsForCache(token);
            foreach (var account in accounts.ToList().Shuffle().Take(2))
            {
                var result = await GetAllCampaigns(account.Name, token);

                await CommitCampaigns(result, token);

                // NOTE: We cannot process all items in one go since it takes
                //       too long, and this is only a secondary function. We
                //       choose 100 items at random and sync them. Over time
                //       all items must be synced eventually.
                foreach (var item in result.Items.ToList().Shuffle().Take(100))
                {
                    try
                    {
                        var result2 = await GetCampaignAllItems(account.Name, item.Id, token);

                        await CommitCampaignItems(result2, token, true);

                        // Prevent spamming.
                        await Task.Delay(250, token);

                        context.MarkProgress(token);
                    }
                    catch (TaskCanceledException)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                }
            }
        }

        /// <summary>
        /// Implements our CRUD interface. This handles
        /// CRUD operations on given objects. The type
        /// of operation and the objects are specified
        /// within the context.
        /// </summary>
        /// <param name="context">CRUD context</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Nothing (task)</returns>
        public Task CreateOrUpdateObjectsAsync(CreateOrUpdateObjectsContext context, CancellationToken token)
        {
            if (context.Entity.Length != 2)
            {
                throw new InvalidOperationException("Two entities expected");
            }

            // First item *must* be an account entity
            if (!(context.Entity[0] is AccountEntity))
            {
                throw new InvalidOperationException("Entity account is expected");
            }

            foreach (var item in context.Entity)
            {
                if (!(item is CampaignEntity || item is AdItemEntity))
                {
                    throw new InvalidOperationException("Entity is invalid for this operation");
                }
            }

            // TODO: 
            // 1.) Convert into Taboola model

            switch (context.Action)
            {
                case Maximiz.Model.CrudAction.Create:
                    if (context.Entity[1] is CampaignEntity)
                    {
                        // TODO: CreateCampaign(account, token);
                    }
                    else if (context.Entity[1] is AdItemEntity)
                    {
                        // TODO: CreateAdItem(account, campaign, token);
                    }
                    else
                    {
                        throw new Exception(); // TODO: Define exception.
                    }
                    break;

                case Maximiz.Model.CrudAction.Read:
                    throw new InvalidOperationException("Read is invalid for this operation");

                case Maximiz.Model.CrudAction.Update:
                    if (context.Entity[1] is CampaignEntity)
                    {
                        // TODO: UpdateCampaign(account, campaign, token);
                    }
                    else if (context.Entity[1] is AdItemEntity)
                    {
                        // TODO: UpdateAdItem(account, campaign, item, token);
                    }
                    else
                    {
                        throw new Exception(); // TODO: Define exception.
                    }
                    break;

                case Maximiz.Model.CrudAction.Delete:
                    if (context.Entity[1] is CampaignEntity)
                    {
                        // TODO: DeleteCampaign(account, campaign, token);
                    }
                    else if (context.Entity[1] is AdItemEntity)
                    {
                        // TODO: DeleteAdItem(account, campaign, item, token);
                    }
                    else
                    {
                        throw new Exception(); // TODO: Define exception.
                    }
                    break;

                case Maximiz.Model.CrudAction.Syncback:
                    if (!(context.Entity[1] is CampaignEntity))
                    {
                        throw new Exception(); // TODO: Define exception.
                    }

                    //var result = await GetCampaigns(account, campaign, token);
                    //await CommitCampaigns(result, token);
                    // for each result.Items
                    //  var result = await GetCampaignItems(account, campaign, item, token);
                    //  await CommitCampaignItems(result, token, true);
                    // endforeach

                    break;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Called upon graceful shutdown.
        /// </summary>
        public void Dispose() => _client?.Dispose();
    }
}
