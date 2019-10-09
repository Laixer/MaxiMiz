using Microsoft.Extensions.Logging;
using Poller.Taboola.Model;
using System;
using System.Threading.Tasks;
using Poller.Taboola.Mapper;
using System.Net.Http;
using System.Threading;

using Account = Maximiz.Model.Entity.Account;
using CampaignInternal = Maximiz.Model.Entity.Campaign;
using AdItemInternal = Maximiz.Model.Entity.AdItem;
using EntityGuid = Maximiz.Model.Entity.Entity<System.Guid>;
using System.Diagnostics;

namespace Poller.Taboola.Traffic
{

    /// <summary>
    /// Responsible for all non-get operations in the Taboola API. This is a 
    /// partial class.
    /// </summary>
    internal partial class CrudExternal
    {

        /// <summary>
        /// Logging interface.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Mapper for campaigns.
        /// </summary>
        private readonly MapperCampaign _mapperCampaign;

        /// <summary>
        /// Mapper for ad items.
        /// </summary>
        private readonly MapperAdItem _mapperAdItem;

        /// <summary>
        /// Mapper for accounts.
        /// </summary>
        private readonly MapperAccount _mapperAccount;

        /// <summary>
        /// Mapper for targets.
        /// </summary>
        private readonly MapperTarget _mapperTarget;

        /// <summary>
        /// Transforms Taboola entities to properly formatted http content objects.
        /// </summary>
        private readonly ContentBuilder _contentBuilder;

        /// <summary>
        /// Used to wrap our http operations.
        /// </summary>
        private readonly HttpWrapper _httpWrapper;

        /// <summary>
        /// Constructor for dependency injection.
        /// TODO Abstraction for mappers
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="httpWrapper">Wrapper for html operations</param>
        public CrudExternal(ILogger logger, HttpWrapper httpWrapper)
        {
            _logger = logger;
            _httpWrapper = httpWrapper;

            _contentBuilder = new ContentBuilder();
            _mapperAccount = new MapperAccount();
            _mapperAdItem = new MapperAdItem();
            _mapperCampaign = new MapperCampaign();
            _mapperTarget = new MapperTarget();
        }

        /// <summary>
        /// Creates a campaign in the Taboola API and returns the created object.
        /// </summary>
        /// <param name="account">The campaign account</param>
        /// <param name="campaign">The core campaign to create external</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The created campaign</returns>
        public async Task<CampaignInternal> CreateCampaignAsync(Account account,
            CampaignInternal campaign, CancellationToken token)
        {
            // Convert
            var converted = _mapperCampaign.Convert(campaign);

            // Create
            var endpoint = $"api/1.0/{account.Name}/campaigns/";
            var content = _contentBuilder.BuildStringContent(converted);
            var campaignExternal = await _httpWrapper.RemoteExecuteAndLogAsync
                <Campaign>(HttpMethod.Post, endpoint, content, token);

            // Convert back and return
            return _mapperCampaign.Convert(campaignExternal, campaign.Id, campaign.CampaignGroupGuid);
        }

        /// <summary>
        /// Updates a campaign in the Taboola API and returns the updated object.
        /// </summary>
        /// <remarks>Also assigns the result to the campaign parameter</remarks>
        /// <param name="account">The campaign account</param>
        /// <param name="campaign">The campaign to update</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The updated campaign</returns>
        public async Task<CampaignInternal> UpdateCampaignAsync(Account account,
            CampaignInternal campaign, CancellationToken token)
        {
            // Validate and convert
            // TODO Why validate?
            ValidateGuid(campaign);
            var converted = _mapperCampaign.Convert(campaign);

            // Update
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaign.SecondaryId}";
            var content = _contentBuilder.BuildStringContent(converted);
            var campaignExternal = await _httpWrapper.RemoteExecuteAndLogAsync
                <Campaign>(HttpMethod.Post, endpoint, content, token);

            // Convert back, assign explicitly and return
            campaign = _mapperCampaign.Convert(campaignExternal, campaign.Id);
            return campaign;
        }

        /// <summary>
        /// Deletes a campaign in the Taboola API and returns the updated object.
        /// </summary>
        /// <remarks>Also assigns the result to the campaign parameter</remarks>
        /// <param name="account">The campaign account</param>
        /// <param name="campaign">The campaign to delete</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The deleted campaign</returns>
        public async Task<CampaignInternal> DeleteCampaignAsync(Account account,
            CampaignInternal campaign, CancellationToken token)
        {
            // Delete
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaign.SecondaryId}/";
            var campaignExternal = await _httpWrapper.RemoteExecuteAndLogAsync
                <Campaign>(HttpMethod.Delete, endpoint, null, token);

            // Convert back, assign explicitly and return
            campaign = _mapperCampaign.Convert(campaignExternal, campaign.Id);
            return campaign;
        }

        /// <summary>
        /// Creates an ad item in the Taboola API. This call will take some time
        /// as the item must be created and approved before the rest of its
        /// fields are populated.
        /// 
        /// TODO These explicit id assignments are dangerous. Reconsider design strategy.
        /// </summary>
        /// <remarks>Also assigns the result to the ad item parameter</remarks>
        /// <param name="account">The ad item account</param>
        /// <param name="adItem">The ad item to create</param>
        /// <param name="campaignId">The campaign Taboola id</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The created and converted ad item</returns>
        public async Task<AdItemInternal> CreateAdItemAsync(Account account,
            AdItemInternal adItem, string campaignId, CancellationToken token)
        {
            // Create an empty ad item
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaignId}/items/";
            var content = _contentBuilder.BuildStringContent(new { url = adItem.Url });
            var adItemExternal = _httpWrapper.RemoteExecuteAndLogAsync<AdItemExternal>(
                HttpMethod.Post, endpoint, content, token).Result;

            // Wait for creation approval
            var adItemWithId = await AwaitAdItemCreationAsync(account, adItemExternal, 
                adItem.Id, adItem.CampaignGuid, token);

            // Update rest of fields
            adItem.SecondaryId = adItemWithId.SecondaryId;
            var adItemWithFields = await UpdateAdItemAsync(account, adItem, campaignId, token);

            // Convert back, assign explicitly and return
            adItemWithFields.Id = adItem.Id;
            adItemWithFields.CampaignGuid = adItem.CampaignGuid;
            return adItemWithFields;
        }

        /// <summary>
        /// Updates an ad item in the Taboola API.
        /// </summary>
        /// <remarks>Also assigns the result to the ad item parameter</remarks>
        /// <param name="account">The ad item account</param>
        /// <param name="adItem">The ad item to update</param>
        /// <param name="campaignId">The campaign Taboola id</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The updated and converted ad item</returns>
        public async Task<AdItemInternal> UpdateAdItemAsync(Account account,
            AdItemInternal adItem, string campaignId, CancellationToken token)
        {
            // Convert
            var converted = _mapperAdItem.Convert(adItem);

            // Update
            var content = _contentBuilder.BuildStringContent(converted, true);
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaignId}/items/{adItem.SecondaryId}/";
            var adItemExternal = await _httpWrapper.RemoteExecuteAndLogAsync
                <AdItemExternal>(HttpMethod.Post, endpoint, content, token);

            // Convert back, assign explicitly and return
            adItem = _mapperAdItem.Convert(adItemExternal, adItem.Id, adItem.CampaignGuid);
            return adItem;
        }

        /// <summary>
        /// Deletes an ad item in the Taboola API.
        /// </summary>
        /// <remarks>Also assigns the result to the ad item parameter</remarks>
        /// <param name="account">The ad item account</param>
        /// <param name="adItem">The ad item to delete</param>
        /// <param name="campaignId">The campaign Taboola id</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The deleted and converted ad item</returns>
        public async Task<AdItemInternal> DeleteAdItemAsync(Account account,
            AdItemInternal adItem, string campaignId, CancellationToken token)
        {
            // Delete
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaignId}/items/{adItem.SecondaryId}";
            var deleted = await _httpWrapper.RemoteExecuteAndLogAsync<AdItemExternal>
                (HttpMethod.Delete, endpoint, null, token);

            // Convert back, assign explicitly and return
            adItem = _mapperAdItem.Convert(deleted, adItem.Id, adItem.CampaignGuid);
            return adItem;
        }

        /// <summary>
        /// Keeps on polling the API to validate that our created ad item has
        /// been validated and left the crawling state. This operation can
        /// take a long time.
        /// </summary>
        /// <remarks>This throws after 30 seconds without result. This also
        /// explicitly assigns the result to the ad item parameter</remarks>
        /// <param name="account">The account</param>
        /// <param name="createdAdItem">The created Taboola ad item</param>
        /// <param name="token">The cancellation token</param>
        /// <param name="adItemGuid">The internal guid of the created item</param>
        /// <param name="campaignGuid">The internal guid of the campaign</param>
        /// <returns>The validated ad item</returns>
        private async Task<AdItemInternal> AwaitAdItemCreationAsync(Account account,
            AdItemExternal createdAdItem, Guid adItemGuid, Guid campaignGuid, CancellationToken token)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.IsRunning)
            {
                var item = await GetAdItemMainAsync(account, createdAdItem.CampaignId, createdAdItem.Id, token);
                var itemConverted = _mapperAdItem.Convert(item);
                var convertedStatus = _mapperAdItem.AdItemStatusToInternal(itemConverted.AdItemStatus);

                // Not crawling means we are done
                if (item != null && convertedStatus != Maximiz.Model.Enums.AdItemStatus.Crawling)
                {
                    item.Id = adItemGuid;
                    item.CampaignGuid = campaignGuid;
                    return item;
                }

                // Else we wait
                else { await Task.Delay(2500); }

                // Set stopwatch boundary
                if (stopwatch.ElapsedMilliseconds > 30000)
                {
                    stopwatch.Stop();
                    break;
                }
            }

            throw new TimeoutException($"Ad item with id = {createdAdItem} " +
                $"was not created after 30 seconds, aborting.");
        }

        /// <summary>
        /// Validates if our entity has a guid.
        /// </summary>
        /// <remarks>Throws if guid is empty or null</remarks>
        /// <param name="entity">The entity to check</param>
        private void ValidateGuid(EntityGuid entity)
        {
            if (entity.Id == Guid.Empty || entity.Id == null)
            {
                throw new NullReferenceException("Entity GUID can't be null or empty");
            }
        }

    }
}
