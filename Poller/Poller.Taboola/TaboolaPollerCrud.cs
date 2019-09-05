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
        /// This first creates the ad item, then populates it with the rest of
        /// the specified parameters. This method can take a long time because
        /// we have to validate whether or not the ad item was created.
        /// 
        /// TODO Might want to split this function #seperation-of-concerns.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="adItem"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task CreateAdItem(AccountCore account,
            AdItemCore adItem, CancellationToken token)
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

    }
}
