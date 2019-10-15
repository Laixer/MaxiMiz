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

using AccountInternal = Maximiz.Model.Entity.Account;
using CampaignInternal = Maximiz.Model.Entity.Campaign;
using AdItemInternal = Maximiz.Model.Entity.AdItem;

using Poller.Taboola.Mapper;
using Poller.Taboola.Traffic;
using System.Collections.Generic;

namespace Poller.Taboola
{

    /// <summary>
    /// Implementation of our Taboola poller.
    /// </summary>
    public class TaboolaPoller : IPollerRefreshAdvertisementData,
        IPollerDataSyncback, IPollerCreateOrUpdateObjects, IDisposable
    {
        private readonly ILogger _logger;
        private readonly HttpWrapper _httpWrapper;

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

            // Create our http wrapper with client
            _httpWrapper = new HttpWrapper(
                _logger,
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

            // Create all our crud objects
            _crudInternal = new CrudInternal(_logger, provider, cache);
            _crudExternal = new CrudExternal(_logger, _httpWrapper);

            // Log version
            _logger.LogInformation($"Poller {Constants.ApplicationName} created with {Constants.ApplicationVersion}.");
        }

        /// <summary>
        /// Implementation of our data refresh interface. This function calls 
        /// the Taboola API and retrieves data about the performance of our ad
        /// items. This is used to retreive metrics, not to validate that all
        /// data we have is is still up to date.
        /// </summary>
        /// <param name="context">The poller context</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task</returns>
        public async Task RefreshAdvertisementDataAsync(PollerContext context,
            CancellationToken token)
        {
            // Get accounts
            var accounts = await _crudInternal.GetAdvertiserAccountsCachedAsync(token);

            // Prevents spamming because of parallel
            var delay = 0;
            var delayIncrement = 250;

            // Process each account parallel
            foreach (var account in accounts.ToList().Shuffle())
            {
                await Task.Delay(delay);
                delay += delayIncrement;

                var adItems = await _crudExternal.GetAdItemsReportFromAccountAsync(account, token);
                await _crudInternal.CommitAdItemReportsBulk(adItems, token);
            }
        }

        /// <summary>
        /// Used for testing.
        /// TODO Should we do this?
        /// </summary>
        /// <param name="context"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task DataSyncbackAsync(PollerContext context, CancellationToken token)
        {
            await DataSyncbackAsync(context, token, null, null, null);
        }

        /// <summary>
        /// Implements our data syncback interface. This is used to check if any
        /// values have been changed in Taboola's own interface. With this we will
        /// always remain up to date.
        /// 
        /// 
        /// TODO Maybe split?
        /// TODO Take parallel into account
        /// TODO Clean up
        /// </summary>
        /// <remarks>
        /// This contains three list pointers. The fetched entities get added
        /// to their respective list, so we can verify the behaviour of this
        /// interface during testing. Only the randomly selected entities get
        /// assigned to the list.
        /// </remarks>
        /// <param name="context">The poller context</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="pointerAccounts">Here we store our accounts</param>
        /// <param name="pointerCampaigns">Here we store our campaigns</param>
        /// <param name="pointerAdItems">Here we store our ad items</param>
        /// <returns>Task</returns>
        public async Task DataSyncbackAsync(PollerContext context, CancellationToken token,
            List<AccountInternal> pointerAccounts, List<CampaignInternal> pointerCampaigns,
            List<AdItemInternal> pointerAdItems)
        {
            // Accounts almost never change, only do this once every 30 iterations.
            if ((context.RunCount + 1) % 30 == 0)
            {
                _logger.LogInformation("Syncback account information");
                var accountsInternal = await _crudExternal.GetAllAccounts(token);
                await _crudInternal.CommitAccountBulk(accountsInternal, token);
            }

            // Get local accounts and extract some campaign data.
            _logger.LogInformation("Syncback campaigns and ad items");
            var accounts = await _crudInternal.GetAdvertiserAccountsCachedAsync(token);

            var selectedAccounts = accounts.ToList().Shuffle().Take(2).ToList();
            if (pointerAccounts != null) { pointerAccounts.AddRange(selectedAccounts); }

            foreach (var account in selectedAccounts.AsParallel())
            {
                await SyncbackAccountCampaignsAsync(account, pointerCampaigns,
                    pointerAdItems, token, context, 5);
            }
        }

        /// <summary>
        /// Syncs back all campaigns for a given account. Also syncs back a 
        /// given amount of campaign items through <see cref="SyncbackCampaignAdItemsAsync"/>.
        /// </summary>
        /// <param name="account">The account to syncback</param>
        /// <param name="pointerCampaigns">Where to store the campaigns</param>
        /// <param name="pointerAdItems">Where to store the campaign ad items</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="context">The poller context</param>
        /// <param name="campaignProcessCount">How many campaigns to process</param>
        /// <returns>Task</returns>
        private async Task SyncbackAccountCampaignsAsync(AccountInternal account,
            List<CampaignInternal> pointerCampaigns, List<AdItemInternal> pointerAdItems,
            CancellationToken token, PollerContext context, int campaignProcessCount)
        {
            // Get and commit all account campaigns
            var campaigns = await _crudExternal.GetAllCampaignsFromAccountAsync(account, token);
            await _crudInternal.CommitCampaignBulk(campaigns, token);

            // Process campaign items for a select amount.
            var selectedCampaigns = campaigns.ToList().Shuffle().Take(campaignProcessCount).ToList();
            if (pointerCampaigns != null) { pointerCampaigns.AddRange(selectedCampaigns); }

            // Also prevent spam
            var delay = 0;
            var delayIncrement = 200;
            foreach (var campaign in selectedCampaigns)
            {
                // Prevent spamming
                await Task.Delay(delay);
                delay += delayIncrement;

                // Process campaign
                await SyncbackCampaignAdItemsAsync(account, campaign, pointerAdItems, token);

                // Mark our progress
                context.MarkProgress(token);
            }
        }

        /// <summary>
        /// Processes syncback for one campaign. This will syncback all ad items 
        /// for this given campaign.
        /// </summary>
        /// <param name="account">The campaign account</param>
        /// <param name="campaign">The campaign</param>
        /// <param name="pointerAdItems">Where to add the ad items to</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        private async Task SyncbackCampaignAdItemsAsync(AccountInternal account,
            CampaignInternal campaign, List<AdItemInternal> pointerAdItems, CancellationToken token)
        {
            // We always need a campaign guid
            if (campaign.Id == null || campaign.Id == Guid.Empty)
            {
                campaign.Id = (await _crudInternal.GetCampaignFromExternalIdAsync(campaign.SecondaryId, token)).Id;
            }

            // Retreive and commit
            var adItems = null as IEnumerable<AdItemInternal>;
            try
            {
                adItems = await _crudExternal.GetAdItemsFromCampaignAsync(account, campaign.SecondaryId, token);
                await _crudInternal.CommitAdItemBulk(adItems, campaign.Id, token, true);
            }
            catch (Exception e) { throw e; }

            // Add to list if present, for testing purposes
            if (pointerAdItems != null) { pointerAdItems.AddRange(adItems); }
        }

        /// <summary>
        /// Implements our CRUD interface. This handles CRUD operations on given
        /// objects. The type of operation and the objects are specified within
        /// the context.
        /// 
        /// TODO This explicit assignment of entity is ugly, fix.
        /// </summary>
        /// <param name="context">CRUD context</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task</returns>
        public async Task CreateOrUpdateObjectsAsync(
            CreateOrUpdateObjectsContext context, CancellationToken token)
        {
            // Throws if invalid
            ValidateCrudContext(context);

            var account = (AccountInternal)context.Entity[0];
            var entity = context.Entity[1];
            switch (context.Action)
            {
                // Create an entity
                // The entity must aleady exist in our own database
                case Maximiz.Model.CrudAction.Create:
                    switch (entity)
                    {
                        case CampaignInternal campaign:
                            campaign = await _crudExternal.CreateCampaignAsync(account, campaign, token);
                            await _crudInternal.UpdateCampaignAsync(campaign, token);
                            context.Entity[1] = campaign;
                            return;

                        case AdItemInternal adItem:
                            var campaignId = (await _crudInternal.GetCampaignFromGuidAsync(adItem.CampaignGuid, token)).SecondaryId; // TODO Use join or view? Can be optimized
                            adItem = await _crudExternal.CreateAdItemAsync(account, adItem, campaignId, token);
                            await _crudInternal.UpdateAdItemAsync(adItem, token);
                            context.Entity[1] = adItem;
                            return;
                    }
                    break;

                // Update an entity
                case Maximiz.Model.CrudAction.Update:
                    switch (entity)
                    {
                        case CampaignInternal campaign:
                            campaign = await _crudExternal.UpdateCampaignAsync(account, campaign, token);
                            await _crudInternal.UpdateCampaignAsync(campaign, token);
                            context.Entity[1] = campaign;
                            return;

                        case AdItemInternal adItem:
                            var campaignId = (await _crudInternal.GetCampaignFromGuidAsync(adItem.CampaignGuid, token)).SecondaryId;
                            adItem = await _crudExternal.UpdateAdItemAsync(account, adItem, campaignId, token);
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
                        case CampaignInternal campaign:
                            campaign = await _crudExternal.DeleteCampaignAsync(account, campaign, token);
                            await _crudInternal.DeleteCampaignAsync(campaign, token);
                            context.Entity[1] = campaign;
                            return;

                        case AdItemInternal adItem:
                            var campaignId = (await _crudInternal.GetCampaignFromGuidAsync(adItem.CampaignGuid, token)).SecondaryId;
                            adItem = await _crudExternal.DeleteAdItemAsync(account, adItem, campaignId, token);
                            await _crudInternal.DeleteAdItemAsync(adItem, token);
                            context.Entity[1] = adItem;
                            return;
                    }
                    break;

                // Syncback for campaign
                case Maximiz.Model.CrudAction.Syncback:
                    switch (entity)
                    {
                        case CampaignInternal campaign:
                            // First refresh the campaign
                            var campaignFetched = await _crudExternal.GetCampaignAsync(account, campaign.SecondaryId, token);
                            campaignFetched.Id = campaign.Id; // TODO Explicit assignment is bad
                            await _crudInternal.UpdateCampaignAsync(campaignFetched, token);

                            // Then refresh all the ad items
                            var adItems = await _crudExternal.GetAdItemsFromCampaignAsync(account, campaign.SecondaryId, token);
                            await _crudInternal.CommitAdItemBulk(adItems, campaign.Id, token);
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
            if (!(account is AccountInternal))
            {
                throw new InvalidOperationException("Account entity is expected as first in array");
            }

            // Check type
            if (!(entity is CampaignInternal || entity is AdItemInternal))
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
                !(entity is CampaignInternal))
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
