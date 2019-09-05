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
        /// Gets a single campaign based on its ID.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="campaign">The campaign</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        private Task<Campaign> GetCampaign(AccountCore account,
            CampaignCore campaign, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaign.SecondaryId}";
            return RemoteQueryAndLogAsync<Campaign>(HttpMethod.Get, endpoint, token);
        }

        /// <summary>
        /// Creates a new campaign in the Taboola API, based on a campaign in
        /// our database. All known parameters will be sent to Taboola.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="campaign">The core campaign</param>
        /// <param name="token">Cancellation token</param>
        private async Task CreateCampaign(AccountCore account, CampaignCore campaign,
            CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns/";
            var content = new StringContent(Json.Serialize(campaign));
            await RemoteExecuteAndLogAsync(HttpMethod.Post, endpoint, content, token);
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
        private async Task CreateAdItem(AccountCore account, AdItemCore adItem,
            CancellationToken token)
        {
            // Get campaign this belongs to
            var campaignId = FetchCampaignIdFromAdItem(adItem, token).Result;

            // First create the ad item
            var endpoint = $"api/1.0 /{account.Name}/campaigns/{campaignId}/items/";
            var content = new StringContent(adItem.Url);
            var createdAdItem = RemoteExecuteAndLogAsync<AdItem>(HttpMethod.Post,
                endpoint, content, token).Result;

            // Validate if the ad item has been created --> not crawling.
            // When this is done we have the populated ad item.
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (true)
            {
                try
                {
                    var result = GetCampaignAllItems(account, campaignId, token).Result.Items;
                    var selected = result.Where(x => x.Id == createdAdItem.Id).FirstOrDefault();

                    // Not crawling means we are done
                    if (selected.CampaignItemStatus != CampaignItemStatus.Crawling)
                    {
                        break;
                    }

                    // Else we wait
                    else
                    {
                        await Task.Delay(2500);
                    }

                    if (stopwatch.ElapsedMilliseconds > 30000)
                    {
                        throw new TimeoutException($"Ad item with id = {createdAdItem} " +
                            $"was not created after 30 seconds, aborting.");
                    }
                }
                catch (Exception e) { }
            }
        }

        /// <summary>
        /// Updates a given campaign from our core database to the taboola API.
        /// </summary>
        /// <param name="account">Account to which the campaign belongs</param>
        /// <param name="campaign">Campaign with parameters</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task</returns>
        private async Task UpdateCampaign(AccountCore account,
            CampaignCore campaign, CancellationToken token)
        {
            var push = _mapperCampaign.Convert(campaign);
            var content = new StringContent(Json.Serialize(push));
            var endpoint = $"api/1.0/{account}/campaigns/{campaign.SecondaryId}";
            await RemoteExecuteAndLogAsync(HttpMethod.Put, endpoint, content, token);
        }

        private async Task UpdateCampaignStatus(string account, string campaign, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account}/campaigns/{campaign}";

            await RemoteQueryAndLogAsync<Campaign>(HttpMethod.Put, endpoint, token);
        }

        private async void DeleteCampaign(string account, string campaign, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}";
            await RemoteQueryAndLogAsync(HttpMethod.Delete, url, token);
        }

        /// <summary>
        /// Query all campaign items for a given campaign from the Taboola API.
        /// The query is based on the campaign id.
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="campaign">Campaign id as string</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task with entity list of ad items</returns>
        private Task<EntityList<AdItem>> GetCampaignAllItems(
            AccountCore account, string campaign, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaign}/items";
            return RemoteQueryAndLogAsync<EntityList<AdItem>>
                (HttpMethod.Get, endpoint, token);
        }

        private Task<AdItem> GetCampaignItem(string account,
            string campaign, string item, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account}/campaigns/{campaign}/items/{item}";
            return RemoteQueryAndLogAsync<AdItem>(HttpMethod.Get, endpoint, token);
        }

        /// <summary>
        /// Performs syncback over a single campaign.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="campaign">The campaign</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Task</returns>
        private async Task SyncbackCampaign(AccountCore account,
            CampaignCore campaign, CancellationToken token)
        {
            // First syncback the campaign
            var campaignApi = await GetCampaign(account, campaign, token);
            var converted = _mapperCampaign.Convert(campaignApi);
            var list = new List<CampaignCore>();
            list.Add(converted);
            await CommitCampaigns(list, token);

            // Then syncback all ad items
            var result = await GetCampaignAllItems(account, campaign, token);
            var items = _mapperAdItem.ConvertAll(result.Items);
            await CommitAdItems(items, token);
        }
    }
}
