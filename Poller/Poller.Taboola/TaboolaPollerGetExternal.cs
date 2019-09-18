using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Poller.Taboola.Model;

using AccountEntity = Maximiz.Model.Entity.Account;
using CampaignEntity = Maximiz.Model.Entity.Campaign;
using AdItemEntity = Maximiz.Model.Entity.AdItem;
using Poller.Helper;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Poller.Taboola
{

    /// <summary>
    /// Partial poller that contains all our external API read operations not
    /// related to entity CRUD operations.
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
            AccountEntity account, CancellationToken token)
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
        private async Task<Campaign> GetCampaignAsync(AccountEntity account,
            string campaignId, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaignId}";
            var result = await RemoteQueryAndLogAsync<Campaign>(HttpMethod.Get, endpoint, token);
            if (string.IsNullOrEmpty(result.Id)) { return null; }
            return result;
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
            AccountEntity account, CampaignEntity campaign, CancellationToken token)
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
           AccountEntity account, string campaignId, CancellationToken token)
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
        private async Task SyncbackCampaignAsync(AccountEntity account,
            CampaignEntity campaign, CancellationToken token)
        {
            // First syncback the campaign
            var campaignApi = await GetCampaignAsync(account, campaign.SecondaryId, token);
            var converted = _mapperCampaign.Convert(campaignApi);
            var list = new List<CampaignEntity>();
            list.Add(converted);
            await CommitCampaigns(list, token);

            // Then syncback all ad items
            var result = await GetCampaignAllItemsAsync(account, campaign, token);
            var items = _mapperAdItem.ConvertAll(result.Items);
            await CommitAdItems(items, token);
        }


    }

}

