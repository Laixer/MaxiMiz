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
using Poller.Taboola.Traffic;

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
        /// Used to perform all operations with our internal database.
        /// </summary>
        private readonly CrudInternal _crudInternal;

        /// <summary>
        /// Used to perform all operations with the external Taboola API.
        /// </summary>
        private readonly CrudExternal _crudExternal;

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

            // Create all our crud objects
            _crudInternal = new CrudInternal(_logger, provider, cache);
            _crudExternal = new CrudExternal(_logger, _httpWrapper);

            // Log version
            _logger.LogInformation($"Poller {Constants.ApplicationName} created with {Constants.ApplicationVersion}.");
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
            var accounts = await _crudInternal.GetAdvertiserAccountsCachedAsync(token);

            foreach (var account in accounts.ToList().Shuffle())
            {
                var adItems = await _crudExternal.GetAdItemsReportFromAccountAsync(account, token);
                await _crudInternal.CommitAdItemBulk(adItems, token);

                // Prevent spamming our database
                await Task.Delay(250, token);
            }
        }

        /// <summary>
        /// Implements our data syncback interface. This is used to check if any
        /// values have been changed in Taboola's own interface. With this we will
        /// always remain up to date.
        /// 
        /// TODO Maybe split?
        /// TODO Take parallel into account
        /// </summary>
        /// <param name="context">The poller context</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task</returns>
        public async Task DataSyncbackAsync(PollerContext context, CancellationToken token)
        {
            // Accounts almost never change, only do this once every 4 iterations.
            if (context.RunCount + 1 % 4 == 0)
            {
                _logger.LogInformation("Syncback account information");
                var accountsExternal = await _crudExternal.GetAllAccounts(token);
                await _crudInternal.CommitAccountBulk(accountsExternal, token);
            }

            // Get local accounts and extract some campaign data.
            _logger.LogInformation("Syncback campaigns and ad items");
            var accounts = await _crudInternal.GetAdvertiserAccountsCachedAsync(token);
            foreach (var account in accounts.ToList().Shuffle().Take(2))
            {
                var campaigns = await _crudExternal.GetAllCampaignsFromAccountAsync(account, token);
                await _crudInternal.CommitCampaignBulk(campaigns, token);

                // Process campaign items for a select amount.
                foreach (var campaign in campaigns.ToList().Shuffle().Take(100)) {

                    var adItems = await _crudExternal.GetAdItemsFromCampaignAsync(account, campaign.SecondaryId, token);
                    await _crudInternal.CommitAdItemBulk(adItems, token, true);

                    // Mark our progress and prevent spamming
                    context.MarkProgress(token);
                    await Task.Delay(250, token);
                }
            }
        }

        /// <summary>
        /// Implements our CRUD interface. This handles CRUD operations on given
        /// objects. The type of operation and the objects are specified within
        /// the context.
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
                            await _crudExternal.CreateCampaignAsync(account, campaign, token);
                            await _crudInternal.UpdateCampaignAsync(campaign, token);
                            context.Entity[1] = campaign;
                            return;

                        case AdItemEntity adItem:
                            var campaignId = (await _crudInternal.GetCampaignFromGuidAsync(adItem.CampaignGuid, token)).SecondaryId;
                            await _crudExternal.CreateAdItemAsync(account, adItem, campaignId, token);
                            await _crudInternal.UpdateAdItemAsync(adItem, token);
                            context.Entity[1] = adItem;
                            return;
                    }
                    break;

                // Update an entity
                case Maximiz.Model.CrudAction.Update:
                    switch (entity)
                    {
                        case CampaignEntity campaign:
                            await _crudExternal.UpdateCampaignAsync(account, campaign, token);
                            await _crudInternal.UpdateCampaignAsync(campaign, token);
                            context.Entity[1] = campaign;
                            return;

                        case AdItemEntity adItem:
                            var campaignId = (await _crudInternal.GetCampaignFromGuidAsync(adItem.CampaignGuid, token)).SecondaryId;
                            await _crudExternal.UpdateAdItemAsync(account, adItem, campaignId, token);
                            await _crudInternal.UpdateAdItemAsync(adItem, token);
                            context.Entity[1] = adItem;
                            return;
                    }
                    break;

                // Delete an entity
                case Maximiz.Model.CrudAction.Delete:
                    switch (entity)
                    {
                        // TODO Do we want to delete this or update this to some delete status? I'd say delete the thing.
                        case CampaignEntity campaign:
                            await _crudExternal.DeleteCampaignAsync(account, campaign, token);
                            await _crudInternal.DeleteCampaignAsync(campaign, token);
                            context.Entity[1] = campaign;
                            return;

                        case AdItemEntity adItem:
                            var campaignId = (await _crudInternal.GetCampaignFromGuidAsync(adItem.CampaignGuid, token)).SecondaryId;
                            await _crudExternal.DeleteAdItemAsync(account, adItem, campaignId, token);
                            await _crudInternal.DeleteAdItemAsync(adItem, token);
                            context.Entity[1] = adItem;
                            return;
                    }
                    break;

                // Syncback for campaign
                case Maximiz.Model.CrudAction.Syncback:
                    switch (entity)
                    {
                        case CampaignEntity campaign:
                            await _crudInternal.UpdateCampaignAsync(await _crudExternal.GetCampaignAsync(account, campaign.SecondaryId, token), token);
                            await _crudInternal.CommitAdItemBulk(await _crudExternal.GetAdItemsFromCampaignAsync(account, campaign.SecondaryId, token), token);
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
        /// 
        public void Dispose() => _httpWrapper.Dispose();
    }
}
