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
using System.Collections.Generic;

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
    internal partial class TaboolaPoller : IPollerRefreshAdvertisementData,
        IPollerDataSyncback, IPollerCreateOrUpdateObjects, IDisposable
    {
        private readonly ILogger _logger;
        private readonly DbProvider _provider;
        private readonly IMemoryCache _cache;
        private readonly HttpManager _client;

        private readonly MapperAccount _mapperAccount;
        private readonly MapperCampaign _mapperCampaign;
        private readonly MapperAdItem _mapperAdItem;
        private readonly MapperTarget _mapperTarget;

        /// <summary>
        /// Constructor with dependency injection.
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="options">The options</param>
        /// <param name="provider">The database provider</param>
        /// <param name="cache">The cache</param>
        public TaboolaPoller(ILoggerFactory logger, TaboolaPollerOptions options,
            DbProvider provider, IMemoryCache cache)
        {
            _logger = logger.CreateLogger(typeof(TaboolaPoller).FullName);
            _provider = provider;
            _cache = cache;

            // Create our http client
            _client = new HttpManager(
                new Uris(options.BaseUrl, "oauth/token", "oauth/token"),
                new OAuthAuthorizationProvider
                {
                    ClientId = options.OAuth2.ClientId,
                    ClientSecret = options.OAuth2.ClientSecret,
                    Username = options.OAuth2.Username,
                    Password = options.OAuth2.Password,
                });

            // Create all our mappers
            _mapperAccount = new MapperAccount();
            _mapperCampaign = new MapperCampaign();
            _mapperAdItem = new MapperAdItem();
            _mapperTarget = new MapperTarget();

            _logger.LogInformation("Taboola poller created");
        }

        /// <summary>
        /// Implementation from our data refresh interface. This function calls 
        /// the Taboola API and retrieves our accounts, campaigns and ad items.
        /// </summary>
        /// <param name="context">The poller context</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task</returns>
        public async Task RefreshAdvertisementDataAsync(PollerContext context,
            CancellationToken token)
        {
            var accounts = await FetchLocalAdvertiserAccountsForCacheAsync(token);

            foreach (var account in accounts.ToList().Shuffle())
            {
                var result = await GetTopCampaignReportAsync(
                    account.Name, token);
                var converted = _mapperAdItem.ConvertAll(result.Items);

                await CommitAdItems(converted, token);

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
                var converted = _mapperAccount.ConvertAll(result.Items);
                await CommitAccounts(converted, token);
            }

            // Get local accounts and extract some campaign data.
            foreach (var account in accounts.ToList().Shuffle().Take(2))
            var accounts = await FetchLocalAdvertiserAccountsForCacheAsync(token);
            {
                _logger.LogDebug($"Extracting data for account: {account.Name}");
                var result = (await GetAllCampaigns(account, token)).Items;
                result = _mapperTarget.ConvertAll(result);
                var converted = _mapperCampaign.ConvertAll(result);

                await CommitCampaigns(converted, token);

                    // NOTE: We cannot process all items in one go since it takes
                    //       too long, and this is only a secondary function. We
                    //       choose 100 items at random and sync them. Over time
                    //       all items must be synced eventually.
                    await ProcessCampainItems(context, account, converted.ToList(), token);
                    // await ProcessCampainItems(context, account, converted.ToList().Shuffle().Take(100), token);
            }
        }

        /// <summary>
        /// This will get all campaign items for each campaign
        /// in the specified list.
        /// </summary>
        /// <param name="context">The poller context</param>
        /// <param name="account">Account object</param>
        /// <param name="campaigns">Campaign list</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        private async Task ProcessCampainItems(
            PollerContext context,
            AccountEntity account,
            IEnumerable<CampaignEntity> campaigns,
            CancellationToken token)
        {
            foreach (var campaign in campaigns)
            {
                try
                {
                    var items = await GetCampaignAllItemsAsync(account, campaign, token);
                    var convertedItems = _mapperAdItem.ConvertAll(items.Items);
                    await CommitAdItems(convertedItems, token, true);
                    _logger.LogDebug($"Processed items for campaign {campaign.SecondaryId}");

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

        /// <summary>
        /// Implements our CRUD interface. This handles CRUD operations on given
        /// objects. The type of operation and the objects are specified within
        /// the context.
        /// 
        /// The context must contain an account and an entity to be updated. 
        /// This function will throw if this is not the case.
        /// 
        /// The entity must either be an ad item or a campaign. No other entites
        /// can be crudded.
        /// 
        /// The operation can not be a read operation.
        /// 
        /// TODO Returning what we created in a clean way.
        /// </summary>
        /// <param name="context">CRUD context</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task</returns>
        public async Task CreateOrUpdateObjectsAsync(
            CreateOrUpdateObjectsContext context, CancellationToken token)
        {
            // Throws if invalid
            ValidateCrudContext(context);

            var account = (AccountEntity)context.Entity[0];
            var entity = context.Entity[1];
            switch (context.Action)
            {
                // Create an entity
                // The entity must aleady exist in our own database
                case Maximiz.Model.CrudAction.Create:
                    switch (entity)
                    {
                        case CampaignEntity campaign:
                            var createdConverted = await CreateCampaignAsync(account, campaign, token);
                            await UpdateLocalCampaignAsync(createdConverted, token);

                            context.Entity[1] = createdConverted;
                            return;

                        case AdItemEntity adItem:
                            // var createdConverted = await CreateAdItemAsync(account, adItem, token);
                            // await UpdateLocalAdItemAsync(createdConverted, token);
                            break;
                    }
                    break;

                // Update an entity
                case Maximiz.Model.CrudAction.Update:
                    switch (entity)
                    {
                        case CampaignEntity campaign:
                            var updated = await UpdateCampaignAsync(account, campaign, token);
                            var updatedConverted = _mapperCampaign.Convert(updated, campaign.Id);
                            await UpdateLocalCampaignAsync(updatedConverted, token);

                            context.Entity[1] = updatedConverted;
                            return;

                        case AdItemEntity adItem:
                            await UpdateAdItemAsync(account, adItem, token);
                            break;
                    }
                    break;
                // Delete an entity
                case Maximiz.Model.CrudAction.Delete:
                    switch (entity)
                    {
                        // TODO Do we want to delete this or update this to some delete status? I'd say delete the thing.
                        case CampaignEntity campaign:
                            var deleted = await DeleteCampaignAsync(account, campaign, token);
                            var deletedConverted = _mapperCampaign.Convert(deleted, campaign.Id);
                            await LocalDeleteCampaignAsync(deletedConverted.Id, token);

                            context.Entity[1] = deletedConverted;
                            return;

                        case AdItemEntity adItem:
                            await DeleteAdItemAsync(account, adItem, token);
                            break;
                    }
                    break;

                // Syncback for campaign
                case Maximiz.Model.CrudAction.Syncback:
                    switch (entity)
                    {
                        case CampaignEntity campaign:
                            await SyncbackCampaignAsync(account, campaign, token);
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Validates if our CRUD context has the correct format.
        /// </summary>
        /// <exception cref="InvalidOperationException">Invalid format</exception>
        /// <param name="context">The CRUD context</param>
        private void ValidateCrudContext(CreateOrUpdateObjectsContext context)
        {
            var account = context.Entity[0];
            var entity = context.Entity[1];
            var action = context.Action;

            // Check format
            if (context.Entity.Length != 2)
            {
                throw new InvalidOperationException("Two entities expected");
            }
            if (!(account is AccountEntity))
            {
                throw new InvalidOperationException("Entity account is expected");
            }

            // Check type
            if (!(entity is CampaignEntity || entity is AdItemEntity))
            {
                throw new InvalidOperationException(
                    "Entity is invalid for this operation");
            }

            // Check operation
            if (action == Maximiz.Model.CrudAction.Read)
            {
                throw new InvalidOperationException(
                    "Read is invalid for this operation");
            }

            // Check syncback
            if (action == Maximiz.Model.CrudAction.Syncback &&
                !(entity is CampaignEntity))
            {
                throw new InvalidOperationException(
                    "Can only syncback for campaigns");
            }
        }

        /// <summary>
        /// Called upon graceful shutdown.
        /// </summary>
        public void Dispose() => _client?.Dispose();
    }
}
