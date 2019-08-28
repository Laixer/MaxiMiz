using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using Poller.Taboola.Model;

using AccountCore = Maximiz.Model.Entity.Account;

namespace Poller.Taboola
{

    /// <summary>
    /// This is the part of our Taboola Poller that
    /// uses http. All outgoing requests are placed
    /// within this file.
    /// </summary>
    internal partial class TaboolaPoller
    {

        /// <summary>
        /// Gets the Top Campaign Reports for a specific date as specified
        /// in the Backstage documentation, deserializes them and  inserts them into the database
        /// </summary>
        private Task<EntityList<AdItemCoResult>> GetTopCampaignReportAsync(string account, CancellationToken token)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["end_date"] = query["start_date"] = DateTime.Now.ToString("yyyy-MM-dd");

            var url = $"api/1.0/{account}/reports/top-campaign-content/dimensions/item_breakdown?{query}";

            return RemoteQueryAndLogAsync<EntityList<AdItemCoResult>>(HttpMethod.Get, url, token);
        }

        /// <summary>
        /// Retrieves accounts from the Taboola API.
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns>Fetched Taboola accounts</returns>
        private Task<EntityList<Account>> GetAllAccounts(CancellationToken token)
        {
            var url = $"api/1.0/users/current/allowed-accounts/";

            return RemoteQueryAndLogAsync<EntityList<Account>>(HttpMethod.Get, url, token);
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
            var url = $"api/1.0/{account.Name}/campaigns";
            return RemoteQueryAndLogAsync<EntityList<Campaign>>
                (HttpMethod.Get, url, token);
        }

        private async Task GetCampaign(string account, string campaign, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}";

            var result = await RemoteQueryAndLogAsync<Campaign>(HttpMethod.Get, url, token);
        }

        private Task<Campaign> CreateCampaign(string account, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/";

            return RemoteQueryAndLogAsync<Campaign>(HttpMethod.Post, url, token);
        }

        private async Task UpdateCampaignStatus(string account, string campaign, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}";

            var result = await RemoteQueryAndLogAsync<Campaign>(HttpMethod.Put, url, token);
        }

        private Task<Campaign> DeleteCampaign(string account, string campaign, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}";

            return RemoteQueryAndLogAsync<Campaign>(HttpMethod.Delete, url, token);
        }

        /// <summary>
        /// Query all campaign items for a given campaign
        /// from the Taboola API.
        /// </summary>
        /// <param name="account">Account name</param>
        /// <param name="campaign">Campaign id name</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        private Task<EntityList<AdItem>> GetCampaignAllItems(
            string account, string campaign, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}/items";

            return RemoteQueryAndLogAsync<EntityList<AdItem>>(HttpMethod.Get, url, token);
        }

        private Task<AdItem> GetCampaignItem(string account, string campaign, string item, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}/items/{item}";

            return RemoteQueryAndLogAsync<AdItem>(HttpMethod.Get, url, token);
        }

        /// <summary>
        /// Run the remote query and catch all exceptions 
        /// where before letting them propagate upwards.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="method">HTTP method.</param>
        /// <param name="url">API endpoint.</param>
        /// <param name="cancellationToken">Cancellation 
        /// token.</param>
        /// <returns>Object of TResult.</returns>
        protected async Task<TResult> RemoteQueryAndLogAsync
            <TResult>(HttpMethod method, string url,
            CancellationToken cancellationToken)
            where TResult : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                _logger.LogTrace($"Querying {method} {url}");

                return await _client.RemoteQueryAsync<TResult>(method, url, cancellationToken);
            }
            catch (Exception e) when (e as OperationCanceledException == null && e as TaskCanceledException == null)
            {
                _logger.LogError($"{url}: {e.Message}");
                throw e;
            }
        }

        /// <summary>
        /// Run the remote execute and catch all exceptions where before letting
        /// them propagate upwards.
        /// </summary>
        /// <param name="url">API endpoint.</param>
        /// <param name="content">Post content.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        protected async Task RemoteExecuteAndLogAsync<TResult>(string url, string content, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                _logger.LogTrace($"Executing {url}");

                await _client.RemoteExecuteAsync(url, new StringContent(content), cancellationToken);
            }
            catch (Exception e) when (e as OperationCanceledException == null && e as TaskCanceledException == null)
            {
                _logger.LogError($"{url}: {e.Message}");
                throw e;
            }
        }

    }
}
