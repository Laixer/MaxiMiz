using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Poller.Taboola.Model;
using Poller.Taboola.Mapper;

using AccountEntity = Maximiz.Model.Entity.Account;
using AdItemEntity = Maximiz.Model.Entity.AdItem;
using CampaignEntity = Maximiz.Model.Entity.Campaign;
using AdGroup = Maximiz.Model.Entity.AdGroup;

namespace Poller.Taboola
{

    /// <summary>
    /// Partial poller that contains all our local database read operations.
    /// </summary>
    internal partial class TaboolaPoller
    {

        /// <summary>
        /// Gets all advertiser accounts from our database.
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns>All advertiser accounts</returns>
        private async Task<IEnumerable<AccountEntity>>
            FetchAdvertiserAccounts(CancellationToken token)
        {
            var sql = @"
                SELECT
                    *
	            FROM
                    public.account
                WHERE
                    publisher = 'taboola'::publisher AND
                    (details::json #>> '{partner_types}')::jsonb ? 'advertiser'";

            using (var connection = _provider.ConnectionScope())
            {
                var result = await connection.QueryAsync<AccountEntity>
                    (new CommandDefinition(sql, cancellationToken: token));
                return result;
            }
        }

        /// <summary>
        /// Gets all publisher accounts from our database.
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns>All publisher accounts</returns>
        private async Task<IEnumerable<AccountEntity>>
            FetchPublisherAccounts(CancellationToken token)
        {
            var sql = @"
                SELECT
                    *
	            FROM
                    public.account
                WHERE
                    publisher = 'taboola'::publisher AND
                    (details::json #>> '{partner_types}')::jsonb ? 'publisher'";

            using (var connection = _provider.ConnectionScope())
            {
                var result = await connection.QueryAsync<AccountEntity>
                    (new CommandDefinition(sql, cancellationToken: token));
                return result;
            }
        }

        /// <summary>
        /// This adds the accounts that are present in
        /// our OWN database to the cache.
        /// TODO: Return account model according to
        /// database scheme.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private Task<IEnumerable<AccountEntity>> FetchLocalAdvertiserAccountsForCacheAsync
            (CancellationToken token)
        {
            return _cache.GetOrCreateAsync(
                "AdvertiserAccounts", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                return await FetchAdvertiserAccounts(token);
            });
        }

        /// <summary>
        /// Gets our campaign id based on an ad item. Ad items belong to an ad 
        /// group, which in term belongs to a campaign.
        /// TODO Maybe use a join? This is not optimal.
        /// TODO This is inefficient. Has to be replaced.
        /// </summary>
        /// <param name="adItem">Integer id of our ad group</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        private async Task<string> FetchCampaignIdFromAdItemAsync(
            AdItemEntity adItem, CancellationToken token)
        {
            // Get the ad group
            var sql = $"SELECT 1 FROM public.ad_group WHERE id = {adItem.AdGroup}";
            var uuid = "";
            using (var connection = _provider.ConnectionScope())
            {
                var result = await connection.QueryAsync<AdGroup>(
                    new CommandDefinition(sql, cancellationToken: token));
                uuid = (result as AdGroup).CampaignUuid;
            }

            // Edge case
            if (string.IsNullOrEmpty(uuid))
            {
                throw new NullReferenceException($"Could not find campaign " +
                    $"for ad item with ad group id {adItem.AdGroup}.");
            }

            // Get the campaign
            var guid = new Guid(uuid);
            return ((await FetchCampaignFromGuidAsync(guid, token)) as CampaignEntity).SecondaryId;
        }

        /// <summary>
        /// Fetches a campaign entity from our local database based on a guid.
        /// When the object can't be retrieved this returns null.
        /// </summary>
        /// <param name="guid">Primary key guid</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The fetched campaign</returns>
        private async Task<CampaignEntity> FetchCampaignFromGuidAsync(
            Guid guid, CancellationToken token)
        {
            try
            {
                var sql = $"SELECT * FROM public.campaign WHERE id::text = '{guid.ToString()}';";
                using (var connection = _provider.ConnectionScope())
                {
                    var result = await connection.QueryAsync<CampaignEntity>(
                        new CommandDefinition(sql, cancellationToken: token));
                    if (result.ToArray().Length > 0) { var x = result.ToArray()[0] as CampaignEntity; return x; }
                    else { return null; }
                }
            }
            catch (Exception e) { throw e; }
        }
    }

}

