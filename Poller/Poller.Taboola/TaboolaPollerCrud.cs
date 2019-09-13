using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using Poller.Taboola.Model;

using AccountCore = Maximiz.Model.Entity.Account;
using CampaignCore = Maximiz.Model.Entity.Campaign;
using AdItemCore = Maximiz.Model.Entity.AdItem;
using Poller.Helper;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace Poller.Taboola
{

    /// <summary>
    /// This is the part of our Taboola Poller that uses http. All CRUD
    /// operations are placed within this file.
    /// </summary>
    internal partial class TaboolaPoller
    {

        /// <summary>
        /// Gets the Top Campaign Reports for a specific date as specified in 
        /// the Backstage documentation, deserializes them and inserts them 
        /// into the database.
        /// </summary>
        private Task<EntityList<AdItemCoResult>> GetTopCampaignReportAsync(
            string account, CancellationToken token)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["end_date"] = query["start_date"] = DateTime.Now.ToString("yyyy-MM-dd");
            var endpoint = $"api/1.0/{account}/reports/top-campaign-content/dimensions/item_breakdown?{query}";
            return RemoteQueryAndLogAsync<EntityList<AdItemCoResult>>(HttpMethod.Get, endpoint, token);
        }

        /// <summary>
        /// Retrieves accounts from the Taboola API.
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns>Fetched Taboola accounts</returns>
        private Task<EntityList<Account>> GetAllAccounts(CancellationToken token)
        {
            var endpoint = $"api/1.0/users/current/allowed-accounts/";
            return RemoteQueryAndLogAsync<EntityList<Account>>(HttpMethod.Get, endpoint, token);
        }

        /// <summary>
        /// Gets all our campaigns for a given account.
        /// </summary>
        /// <param name="account">The core account</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>The campaign list</returns>
        private Task<EntityList<Campaign>> GetAllCampaigns(
            AccountCore account, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns";
            return RemoteQueryAndLogAsync<EntityList<Campaign>>
                (HttpMethod.Get, endpoint, token);
        }

        /// <summary>
        /// Gets a single campaign based on its ID in the Taboola API.
        /// </summary>
        /// <remarks>This returns null if the Taboola API returns a campaign
        /// with Id set to null, indicating it does not exist.</remarks>
        /// <param name="account">The account</param>
        /// <param name="campaignId">The campaign id</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        private async Task<Campaign> GetCampaignAsync(AccountCore account,
            string campaignId, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaignId}";
            return RemoteQueryAndLogAsync<Campaign>(HttpMethod.Get, endpoint, token);
        }

        /// <summary>
        /// Creates a new campaign in the Taboola API, based on a campaign in
        /// our database. All known parameters will be sent to Taboola. After 
        /// creation the Taboola object is converted and returned, along with
        /// the original GUID.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="campaign">The core campaign</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>A new campaign object converted from the created campaign
        /// along with the correct GUID</returns>
        private async Task<CampaignCore> CreateCampaignAsync(AccountCore account,
            CampaignCore campaign, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns/";
            var converted = _mapperCampaign.Convert(campaign);
            var content = BuildStringContent(converted);
            var result = await RemoteExecuteAndLogAsync<Campaign>(HttpMethod.Post, endpoint, content, token);
            var campaignFetched = _mapperCampaign.Convert(result);
            await UpdateCampaign(account, campaignFetched, token);
        }

        /// <summary>
        /// This first creates the ad item, then waits for Taboola to set
        /// the ad item status to anything but crawling. This method can 
        /// take a long time because we have to validate whether or not 
        /// the ad item was created. 
        /// 
        /// The created ad item is converted and inserted into our own 
        /// database after its creation.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="adItem">The ad item</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        private async Task CreateAdItemAsync(AccountCore account, AdItemCore adItem,
            CancellationToken token)
        {
            // Get campaign this belongs to
            var campaignId = FetchCampaignIdFromAdItemAsync(adItem, token).Result;

            // First create the ad item
            var endpoint = $"api/1.0 /{account.Name}/campaigns/{campaignId}/items/";
            var content = BuildStringContent(adItem.Url);
            var createdAdItem = RemoteExecuteAndLogAsync<AdItem>(HttpMethod.Post,
                endpoint, content, token).Result;

            // Validate if the ad item has been created --> not crawling.
            var createdWithFields = await ValidateAdItemAsync(account, campaignId, createdAdItem, token);
            await CommitAdItem(createdWithFields, token);
        }

        /// <summary>
        /// Keeps on polling the API to validate that our created ad item has
        /// been validated and left the crawling state. This operation can
        /// take a long time.
        /// </summary>
        /// <remarks>This throws after 30 seconds without result</remarks>
        /// <param name="account">The account</param>
        /// <param name="campaignId">The numeric id as string of the campaign
        /// this ad item belolngs to</param>
        /// <param name="createdAdItem">The created Taboola ad item</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        private async Task<AdItemCore> ValidateAdItemAsync(AccountCore account,
            string campaignId, AdItem createdAdItem, CancellationToken token)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.IsRunning)
            {
                var result = GetCampaignAllItems(account, campaignId, token).Result.Items;
                var selected = result.Where(x => x.Id == createdAdItem.Id).FirstOrDefault();

                // Not crawling means we are done
                if (selected.CampaignItemStatus != CampaignItemStatus.Crawling)
                {
                    return _mapperAdItem.Convert(selected);
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
        /// Updates a given campaign from our core database to the taboola API.
        /// The campaign entity MUST have a GUID attached to it.
        /// </summary>
        /// <param name="account">Account to which the campaign belongs</param>
        /// <param name="campaign">Campaign with parameters</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task</returns>
        private async Task<Campaign> UpdateCampaignAsync(AccountCore account,
            CampaignCore campaign, CancellationToken token)
        {
            var push = _mapperCampaign.Convert(campaign);
            var content = BuildStringContent(push);
            var endpoint = $"api/1.0/{account}/campaigns/{campaign.SecondaryId}";
            await RemoteExecuteAndLogAsync(HttpMethod.Put, endpoint, content, token);
        }

        /// <summary>
        /// Updates an ad item in the Taboola API.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="adItem">The ad item</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        private async Task UpdateAdItemAsync(AccountCore account, AdItemCore adItem,
            CancellationToken token)
        {
            var content = BuildStringContent(adItem);
            var campaignId = await FetchCampaignIdFromAdItem(adItem, token);
            var endpoint = $"api/1.0/{account}/campaigns/{campaignId}/{adItem.SecondaryId}";
            await RemoteExecuteAndLogAsync(HttpMethod.Post, endpoint, content, token);
        }

        /// <summary>
        /// Deletes a campaign from the Taboola API.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="campaign">The campaign</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        private async Task<Campaign> DeleteCampaignAsync(AccountCore account,
            CampaignCore campaign, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaign.SecondaryId}";
            await RemoteExecuteAndLogAsync(HttpMethod.Delete, endpoint, null, token);
        }

        /// <summary>
        /// Deletes an ad item from the Taboola API.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="adItem"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task DeleteAdItemAsync(AccountCore account, AdItemCore adItem,
            CancellationToken token)
        {
            var campaignId = await FetchCampaignIdFromAdItemAsync(adItem, token);
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaignId}/items/{adItem.SecondaryId}";
            await RemoteExecuteAndLogAsync(HttpMethod.Delete, endpoint, null, token);
        }

        /// <summary>
        /// Query all campaign items for a given campaign from the Taboola API.
        /// The query is based on the campaign id.
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="campaign">Campaign</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task with entity list of ad items</returns>
        private Task<EntityList<AdItem>> GetCampaignAllItemsAsync(
            AccountCore account, CampaignCore campaign, CancellationToken token)
        {
            return GetCampaignAllItemsAsync(account, campaign.SecondaryId, token);
        }

        /// <summary>
        /// Query all campaign items for a given campaign from the Taboola API.
        /// The query is based on the campaign id.
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="campaignId">Campaign id as string</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task with entity list of ad items</returns>
        private Task<EntityList<AdItem>> GetCampaignAllItemsAsync(
           AccountCore account, string campaignId, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaignId}/items";
            return RemoteQueryAndLogAsync<EntityList<AdItem>>(HttpMethod.Get, endpoint, token);
        }

        /// <summary>
        /// Performs syncback over a single campaign.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="campaign">The campaign</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        private async Task SyncbackCampaignAsync(AccountCore account,
            CampaignCore campaign, CancellationToken token)
        {
            // First syncback the campaign
            var campaignApi = await GetCampaignAsync(account, campaign.SecondaryId, token);
            var converted = _mapperCampaign.Convert(campaignApi);
            var list = new List<CampaignCore>();
            list.Add(converted);
            await CommitCampaigns(list, token);

            // Then syncback all ad items
            var result = await GetCampaignAllItemsAsync(account, campaign, token);
            var items = _mapperAdItem.ConvertAll(result.Items);
            await CommitAdItems(items, token);
        }

        /// <summary>
        /// Does a proper encoding of any object. This returns a JSON string
        /// content object with UTF-8 encoding and application/json added to
        /// it.
        /// </summary>
        /// <param name="obj">The object to serialize</param>
        /// <returns>Stringcontent object</returns>
        private StringContent BuildStringContent(object obj)
        {
            var serialized = Json.Serialize(obj);
            return new StringContent(serialized, Encoding.UTF8, "application/json");
        }
    }
}
