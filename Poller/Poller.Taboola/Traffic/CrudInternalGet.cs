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
