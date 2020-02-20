using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CampaignInternal = Maximiz.Model.Entity.Campaign;
using AccountInternal = Maximiz.Model.Entity.Account;
using AdItemInternal = Maximiz.Model.Entity.AdItem;
using Microsoft.Extensions.Caching.Memory;
using Poller.Taboola.Model;

namespace Poller.Taboola.Traffic
{
    /// <summary>
    /// Responsible for performing read operations from our own database. This 
    /// is a partial class.
    /// </summary>
    internal partial class CrudInternal
    {
        /// <summary>
        /// Gets a campaign from our local database based on the GUID.
        /// </summary>
        /// <remarks>Returns null if item is not found</remarks>
        /// <param name="guid">The campaign GUID</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Our internal database campaign</returns>
        public async Task<CampaignInternal> GetCampaignFromGuidAsync(
            Guid guid, CancellationToken token)
        {
            var sql = $"SELECT * FROM public.campaign WHERE id::text = '{guid.ToString()}';";
            using (var connection = _dbProvider.ConnectionScope())
            {
                var result = await connection.QueryAsync<CampaignInternal>(
                    new CommandDefinition(sql, cancellationToken: token));
                if (result.ToArray().Length > 0)
                {
                    var x = result.ToArray()[0] as CampaignInternal;
                    return x;
                }
                else { return null; }
            }
        }

        /// <summary>
        /// Gets a campaign based on its external id.
        /// TODO Can this ever conflict? YES --> when using other publishers as well secondary_id can overlap --> add publisher to query too
        /// </summary>
        /// <param name="externalId">The external taboola string id</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The campaign</returns>
        public async Task<CampaignInternal> GetCampaignFromExternalIdAsync(
            string externalId, CancellationToken token)
        {
            var sql = $"SELECT * FROM public.campaign WHERE secondary_id = '{externalId}';";
            using (var connection = _dbProvider.ConnectionScope())
            {
                var result = await connection.QueryAsync<CampaignInternal>(
                    new CommandDefinition(sql, cancellationToken: token));
                if (result.ToArray().Length > 0)
                {
                    var x = result.ToArray()[0] as CampaignInternal;
                    return x;
                }
                else { return null; }
            }
        }

        /// <summary>
        /// Gets an ad item from our local database based on the GUID.
        /// </summary>
        /// <remarks>Returns null if item is not found</remarks>
        /// <param name="guid">The ad item GUID</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Our internal database ad item</returns>
        public async Task<AdItemInternal> GetAdItemFromGuidAsync(
            Guid guid, CancellationToken token)
        {
            var sql = $"SELECT * FROM public.ad_item WHERE id::text = '{guid.ToString()}';";
            using (var connection = _dbProvider.ConnectionScope())
            {
                var result = await connection.QueryAsync<AdItemInternal>(
                    new CommandDefinition(sql, cancellationToken: token));
                if (result.ToArray().Length > 0)
                {
                    var x = result.ToArray()[0] as AdItemInternal;
                    return x;
                }
                else { return null; }
            }
        }

        /// <summary>
        /// Gets an ad item from our local database based on the GUID.
        /// </summary>
        /// <remarks>Returns null if item is not found</remarks>
        /// <param name="externalId">The external id</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>Our internal database ad item</returns>
        public async Task<AdItemInternal> GetAdItemFromExternalIdAsync(
            string externalId, CancellationToken token)
        {
            var sql = $"SELECT * FROM public.ad_item WHERE secondary_id = '{externalId}';";
            using (var connection = _dbProvider.ConnectionScope())
            {
                var result = await connection.QueryAsync<AdItemInternal>(
                    new CommandDefinition(sql, cancellationToken: token));
                if (result.ToArray().Length > 0)
                {
                    var x = result.ToArray()[0] as AdItemInternal;
                    return x;
                }
                else { return null; }
            }
        }

        /// <summary>
        /// Gets all advertiser accounts from our cache. If this data is not
        /// present, it is polled from our local database and stored in the
        /// cache.
        /// </summary>
        /// <param name="token">The cancellation token</param>
        /// <returns>All advertiser accounts</returns>
        public Task<IEnumerable<AccountInternal>>
            GetAdvertiserAccountsCachedAsync(CancellationToken token)
        {
            return GetAccountsCachedAsync(AccountType.Advertiser, token);
        }

        /// <summary>
        /// Gets all publisher accounts from our cache. If this data is not
        /// present, it is polled from our local database and stored in the
        /// cache.
        /// </summary>
        /// <param name="token">The cancellation token</param>
        /// <returns>All publisher accounts</returns>
        public Task<IEnumerable<AccountInternal>>
            GetPublisherAccountsCachedAsync(CancellationToken token)
        {
            return GetAccountsCachedAsync(AccountType.Publisher, token);
        }

        /// <summary>
        /// Gets all accounts of specified type from our cache. If this data
        /// is not present, it is polled from our local database and stored in 
        /// the cache.
        /// TODO This is not bulletproof for long enum names
        /// </summary>
        /// <param name="accountType">The account type</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>All publisher accounts</returns>
        private async Task<IEnumerable<AccountInternal>> GetAccountsCachedAsync(
            AccountType accountType, CancellationToken token)
        {
            return await _cache.GetOrCreateAsync(
                "AdvertiserAccounts", async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromDays(1);

                    var sql = @"
                        SELECT
                            *
	                    FROM
                            public.account
                        WHERE
                            publisher = 'taboola'::publisher AND
                            (details::json #>> '{partner_types}')::jsonb ? '"
                            + accountType.ToString().ToLower() + "'";

                    using (var connection = _dbProvider.ConnectionScope())
                    {
                        return await connection.QueryAsync<AccountInternal>
                            (new CommandDefinition(sql, cancellationToken: token));
                    }
                });
        }
       
    }
}
