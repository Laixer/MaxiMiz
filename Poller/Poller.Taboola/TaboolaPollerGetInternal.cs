using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Caching.Memory;

using AccountEntity = Maximiz.Model.Entity.Account;
using AdItemEntity = Maximiz.Model.Entity.AdItem;
using CampaignEntity = Maximiz.Model.Entity.Campaign;
using AdGroup = Maximiz.Model.Entity.AdGroup;
using CampaignGroup = Maximiz.Model.Entity.CampaignGroup;

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
        /// Fetches a campaign entity from our local database based on a guid.
        /// When the object can't be retrieved this returns null.
        /// </summary>
        /// <param name="guid">Primary key guid</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The fetched campaign</returns>
        private async Task<CampaignEntity> GetLocalCampaignFromGuidAsync(
            Guid guid, CancellationToken token)
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

        /// <summary>
        /// Gets all ad groups that are linked to a given campaign.
        /// TODO Joins.
        /// TODO Do we ever need this?
        /// </summary>
        /// <param name="campaign">The campaign</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>A (possibly empty) list of linked ad groups</returns>
        private async Task<IEnumerable<AdGroup>> GetLocalAdGroupsFromCampaignAsync(
            CampaignEntity campaign, CancellationToken token)
        {
            var campaignGroupIds = null as IEnumerable<int>;
            using (var connection = _provider.ConnectionScope())
            {
                var sqlIds = $"SELECT * FROM public.campaign_group_ad_group WHERE campaign_group::text = '{campaign.Id.ToString()}';";
                campaignGroupIds = await connection.QueryAsync<int>(
                    new CommandDefinition(sqlIds, cancellationToken: token));
                if (campaignGroupIds.ToList().Count == 0) { return new List<AdGroup>(); }
            }

            using (var connection = _provider.ConnectionScope())
            {
                var sqlAdGroups = $"SELECT * FROM public.ad_group WHERE id = @id";
                // TODO Intermediate result
                var result = await connection.QueryAsync<AdGroup>(
                    new CommandDefinition(sqlAdGroups, campaignGroupIds, cancellationToken: token));
                return result;
            }
        }

        /// <summary>
        /// Gets a list of all ad items from a given list of ad group that belong to
        /// a given campaign.
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="adGroups"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<IEnumerable<AdItemEntity>> GetLocalAdItemsFromAdGroupsAsync(
            CampaignEntity campaign, IEnumerable<AdGroup> adGroups, CancellationToken token)
        {
            using (var connection = _provider.ConnectionScope())
            {
                var sqlIds = $"SELECT * FROM public.ad_item WHERE campaign_id = {campaign.CampaignGroup} AND ad_group = @Id";

                // TODO intermediate result
                var result = await connection.QueryAsync<AdItemEntity>(
                    new CommandDefinition(sqlIds, adGroups, cancellationToken: token));
                return result;
            }
        }

        private async Task<IEnumerable<AdItemEntity>> GetLocalAdItemsFromCampaignAsync(
            CampaignEntity campaign, CancellationToken token)
        {
            throw new NotImplementedException("GetLocalAdItemsFromCampaignAsync not implemented");
        }

        private async Task<bool> IsCampaignGroupAdGroupLinkedAsync(CampaignGroup campaignGroup, AdGroup adGroup, CancellationToken token)
        {
            throw new NotImplementedException("IsCampaignGroupAdGroupLinked not implemented");
        }

        private async Task<IEnumerable<CampaignEntity>> GetLocalCampaignsFromCampaignGroupAsync(
            CampaignGroup campaignGroup, CancellationToken token)
        {
            throw new NotImplementedException("GetLocalCampaignsFromCampaignGroupAsync not implemented");
        }

        private async Task<IEnumerable<AdItemEntity>> GetLocalAdItemsCampaignAdGroupAsync(
        CampaignEntity campaign, AdGroup adGroup, CancellationToken token)
        {
            throw new NotImplementedException("GetLocalAdItemsCampaignAdGroupAsync not implemented");
        }
    }

}

