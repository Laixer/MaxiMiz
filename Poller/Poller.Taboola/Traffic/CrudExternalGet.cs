using Poller.Taboola.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AccountInternal = Maximiz.Model.Entity.Account;
using CampaignInternal = Maximiz.Model.Entity.Campaign;
using AdItemInternal = Maximiz.Model.Entity.AdItem;
using System.Net.Http;
using System.Web;

namespace Poller.Taboola.Traffic
{

    /// <summary>
    /// Responsible for get operations from the Taboola API. This is a partial
    /// class.
    /// </summary>
    internal partial class CrudExternal
    {

        /// <summary>
        /// Retrieves all accessible accounts from the Taboola API.
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns>Fetched converted accounts</returns>
        private async Task<IEnumerable<AccountInternal>> GetAllAccounts(CancellationToken token)
        {
            var endpoint = $"api/1.0/users/current/allowed-accounts/";
            var accountsExternal = await _httpWrapper.RemoteQueryAndLogAsync
                <EntityList<Account>>(HttpMethod.Get, endpoint, token);

            // Convert and return
            return _mapperAccount.ConvertAll(accountsExternal.Items);
        }

        /// <summary>
        /// Gets all our onverted campaigns for a given account. This also deals
        /// with the correct conversion of our target objects.
        /// </summary>
        /// <param name="account">The core account</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>The converted campaign list</returns>
        private async Task<IEnumerable<CampaignInternal>> GetAllCampaigns(
            AccountInternal account, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns";
            var campaignsExternal = await _httpWrapper.RemoteQueryAndLogAsync
                <EntityList<Campaign>>(HttpMethod.Get, endpoint, token);

            // Convert and return
            campaignsExternal = _mapperTarget.ConvertAll(campaignsExternal);
            return _mapperCampaign.ConvertAll(campaignsExternal);
        }

        /// <summary>
        /// Gets a campaign from the Taboola API based on its external id.
        /// </summary>
        /// <remarks>This has no method of retreiving the campaign GUID. This
        /// returns null if the campaign is not present in the API.</remarks>
        /// <param name="account">The campaign account</param>
        /// <param name="externalCampaignId">The campaign Taboola id</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The converted campaign</returns>
        public async Task<CampaignInternal> GetCampaignAsync(AccountInternal account,
            string externalCampaignId, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns/{externalCampaignId}";
            var campaignExternal = await _httpWrapper.RemoteQueryAndLogAsync
                <Campaign>(HttpMethod.Get, endpoint, token);

            // Return null if campaign was non existent
            if (string.IsNullOrEmpty(campaignExternal.Id)) { return null; }

            // Convert and return
            campaignExternal = _mapperTarget.ConvertTarget(campaignExternal);
            return _mapperCampaign.Convert(campaignExternal);
        }

        /// <summary>
        /// Gets the ad item statistics for a given campaign from the Taboola
        /// API. This also converts the items back to our data model.       
        /// 
        /// TODO We can't merge ad items at the moment!
        /// </summary>
        /// <param name="account">The campaign Taboola account</param>
        /// <param name="campaignExternalId">The external campaign id</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The converted ad items</returns>
        public async Task<IEnumerable<AdItemInternal>> GetAdItemsReportFromCampaignAsync(
            AccountInternal account, string campaignExternalId, CancellationToken token)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["end_date"] = query["start_date"] = DateTime.Now.ToString("yyyy-MM-dd");
            var endpoint = $"api/1.0/{account}/reports/top-campaign-content/dimensions/item_breakdown?{query}";
            var adItemsExternal = await _httpWrapper.RemoteQueryAndLogAsync
                <EntityList<AdItemReports>>(HttpMethod.Get, endpoint, token);

            // Convert and return
            return _mapperAdItem.ConvertAll(adItemsExternal.Items);
        }

        /// <summary>
        /// Gets all ad items that belong to a given campaign id and converts
        /// them to our internal data model.
        /// </summary>
        /// <param name="account">The campaign account</param>
        /// <param name="campaignExternalId">The campaign Taboola id</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>A list of converted ad items</returns>
        public async Task<IEnumerable<AdItemInternal>> GetAdItemsFromCampaignAsync(
            AccountInternal account, string campaignExternalId, CancellationToken token)
        {
            // Get the ad items
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaignExternalId}/items";
            var externalAdItems = await _httpWrapper.RemoteQueryAndLogAsync
                <EntityList<AdItemMain>>(HttpMethod.Get, endpoint, token);

            // Convert and return
            return _mapperAdItem.ConvertAll(externalAdItems.Items);
        }

        /// <summary>
        /// Gets a single ad item from the Taboola API and converts it.
        /// TODO Merging ad items
        /// </summary>
        /// <param name="account">The campaign Taboola account</param>
        /// <param name="campaignId">The campaign Taboola id</param>
        /// <param name="adItemId">The ad item Taboola id</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The converted ad item</returns>
        public async Task<AdItemInternal> GetAdItemAsync(AccountInternal account,
            string campaignId, string adItemId, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns/{campaignId}/items/{adItemId}";
            var adItemExternal = await _httpWrapper.RemoteQueryAndLogAsync
                <AdItemReports>(HttpMethod.Get, endpoint, token);

            // Return null if campaign was non existent
            if (string.IsNullOrEmpty(adItemExternal.Id)) { return null; }

            // Convert and return
            return _mapperAdItem.ConvertAdditional(adItemExternal);
        }

    }
}
