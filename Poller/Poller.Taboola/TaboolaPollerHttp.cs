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
    /// This is the part of our Taboola Poller that uses http. All outgoing 
    /// requests are placed within this file. All http requests are handled
    /// by our <see cref="HttpManager"/>.
    /// </summary>
    internal partial class TaboolaPoller
    {

        /// <summary>
        /// Gets the Top Campaign Reports for a specific date as specified in 
        /// the Backstage documentation, deserializes them and inserts them 
        /// into the database.
        /// </summary>
        private Task<EntityList<AdItemCoResult>> GetTopCampaignReportAsync(string account, CancellationToken token)
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

        private async Task GetCampaign(string account, string campaign, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account}/campaigns/{campaign}";

            var result = await RemoteQueryAndLogAsync<Campaign>(HttpMethod.Get, endpoint, token);
        }

        /// <summary>
        /// Creates a new campaign in the Taboola API.
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>The created campaign</returns>
        private Task<Campaign> CreateCampaign(
            AccountCore account, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account.Name}/campaigns/";
            return RemoteQueryAndLogAsync<Campaign>(
                HttpMethod.Post, endpoint, token);
        }

        private async Task UpdateCampaignStatus(string account, string campaign, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account}/campaigns/{campaign}";

            var result = await RemoteQueryAndLogAsync<Campaign>(HttpMethod.Put, endpoint, token);
        }

        private Task<Campaign> DeleteCampaign(string account, string campaign, CancellationToken token)
        {
            var url = $"api/1.0/{account}/campaigns/{campaign}";

            return RemoteQueryAndLogAsync<Campaign>(HttpMethod.Delete, url, token);
        }

        /// <summary>
        /// Query all campaign items for a given campaign from the Taboola API.
        /// </summary>
        /// <param name="account">Account name</param>
        /// <param name="campaign">Campaign id name</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task with entity list of ad items</returns>
        private Task<EntityList<AdItem>> GetCampaignAllItems(
            string account, string campaign, CancellationToken token)
        {
            var endpoint = $"api/1.0/{account}/campaigns/{campaign}/items";
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
        /// Run the remote query and catch all exceptions  before letting them 
        /// propagate upwards.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="method">HTTP method.</param>
        /// <param name="endpoint">API endpoint.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task with TResult object</returns>
        protected async Task<TResult> RemoteQueryAndLogAsync
            <TResult>(HttpMethod method, string endpoint,
            CancellationToken cancellationToken)
            where TResult : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                _logger.LogTrace($"Querying {method} {endpoint}");

                return await _client.RemoteQueryAsync<TResult>(method, endpoint, cancellationToken);
            }
            catch (Exception e) when (e as OperationCanceledException == null && e as TaskCanceledException == null)
            {
                _logger.LogError($"{endpoint}: {e.Message}");
                throw e;
            }
        }

        /// <summary>
        /// Run the remote execute and catch all exceptions where before letting
        /// them propagate upwards.
        /// </summary>
        /// <param name="method">Http method</param>
        /// <param name="endpoint">API endpoint</param>
        /// <param name="content">Http content</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        protected async Task RemoteExecuteAndLogAsync<TResult>(
            HttpMethod method, string endpoint, HttpContent content, 
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                _logger.LogTrace($"Executing {endpoint} with content {content.ToString()}");

                await _client.RemoteExecuteAsync(method, endpoint, content, cancellationToken);
            }
            catch (Exception e) when (e as OperationCanceledException == null && e as TaskCanceledException == null)
            {
                _logger.LogError($"{endpoint}: {e.Message}");
                throw e;
            }
        }

    }
}
