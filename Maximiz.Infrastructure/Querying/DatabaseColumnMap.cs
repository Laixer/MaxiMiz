using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Maximiz.Infrastructure.Querying
{

    /// <summary>
    /// Maps our propety names to our database column names.
    /// </summary>
    internal static class DatabaseColumnMap
    {

        private static readonly Dictionary<string, string> MapAccount = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> MapCampaign = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> MapCampaignWithStats = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> MapCampaignGroup = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> MapCampaignGroupWithStats = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> MapAdGroup = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> MapAdGroupWithStats = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> MapAdItem = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> MapAdItemWithStats = new Dictionary<string, string>();

        /// <summary>
        /// Called once to setup the map.
        /// </summary>
        static DatabaseColumnMap()
        {
            // Account
            MapAccount.Add(PropertyUtility.GetName<Account>((x) => x.Currency), "currency");
            MapAccount.Add(PropertyUtility.GetName<Account>((x) => x.Details), "details");
            MapAccount.Add(PropertyUtility.GetName<Account>((x) => x.Id), "id");
            MapAccount.Add(PropertyUtility.GetName<Account>((x) => x.Name), "name");
            MapAccount.Add(PropertyUtility.GetName<Account>((x) => x.Publisher), "publisher");
            MapAccount.Add(PropertyUtility.GetName<Account>((x) => x.SecondaryId), "secondary_id");

            // Campaign
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.AccountGuid), "branding_text");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.ApprovalState), "approval_state");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.BidStrategy), "bid_strategy");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.BrandingText), "branding_text");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.Budget), "budget");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.BudgetDaily), "budget_daily");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.BudgetModel), "budget_model");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.CampaignGroupGuid), "campaign_group_guid");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.ConnectionTypes), "connection_types");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.CreateDate), "create_date");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.DeleteDate), "delete_date");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.Delivery), "delivery");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.Details), "details");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.Devices), "devices");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.EndDate), "end_date");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.Id), "id");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.InitialCpc), "initial_cpc");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.Language), "language");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.LocationExclude), "location_exclude");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.LocationInclude), "location_include");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.Name), "name");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.Note), "note");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.OperatingSystems), "operating_systems");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.Publisher), "publisher");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.SecondaryId), "secondary_id");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.Spent), "spent");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.StartDate), "start_date");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.Status), "status");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.TargetUrl), "target_url");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.UpdateDate), "update_date");
            MapCampaign.Add(PropertyUtility.GetName<Campaign>((x) => x.Utm), "utm");

            // Campaign with stats
            MapCampaignWithStats.AddAll(MapCampaign);
            MapCampaignWithStats.Add(PropertyUtility.GetName<CampaignWithStats>((x) => x.Actions), "actions");
            MapCampaignWithStats.Add(PropertyUtility.GetName<CampaignWithStats>((x) => x.Clicks), "clicks");
            MapCampaignWithStats.Add(PropertyUtility.GetName<CampaignWithStats>((x) => x.Ctr), "ctr");
            MapCampaignWithStats.Add(PropertyUtility.GetName<CampaignWithStats>((x) => x.Profit), "profit");
            MapCampaignWithStats.Add(PropertyUtility.GetName<CampaignWithStats>((x) => x.Revenue), "revenue");
            MapCampaignWithStats.Add(PropertyUtility.GetName<CampaignWithStats>((x) => x.RevenueAdsense), "revenue_adsense");
            MapCampaignWithStats.Add(PropertyUtility.GetName<CampaignWithStats>((x) => x.RevenueTaboola), "revenue_taboola");
            MapCampaignWithStats.Add(PropertyUtility.GetName<CampaignWithStats>((x) => x.Roi), "roi");

            // Campaign group
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.ApprovalState), "approval_state");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.BrandingText), "branding_text");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.Budget), "budget");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.BudgetDaily), "budget_daily");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.CreateDate), "create_date");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.DeleteDate), "delete_date");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.Delivery), "delivery");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.EndDate), "end_date");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.Id), "id");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.InitialCpc), "initial_cpc");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.LocationExclude), "location_exclude");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.LocationInclude), "location_include");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.Name), "name");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.Publisher), "publisher");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.Spent), "spent");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.StartDate), "start_date");
            MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroup>((x) => x.UpdateDate), "update_date");

            // Campaign group with stats
            MapCampaignGroupWithStats.AddAll(MapCampaignGroup);
            //MapCampaignGroup.Add(PropertyUtility.GetName<CampaignGroupWithStats>((x) => x.), "");

            // Ad group
            MapAdGroup.Add(PropertyUtility.GetName<AdGroup>((x) => x.CreateDate), "create_date");
            MapAdGroup.Add(PropertyUtility.GetName<AdGroup>((x) => x.DeleteDate), "delete_date");
            MapAdGroup.Add(PropertyUtility.GetName<AdGroup>((x) => x.Description), "description");
            MapAdGroup.Add(PropertyUtility.GetName<AdGroup>((x) => x.Id), "id");
            MapAdGroup.Add(PropertyUtility.GetName<AdGroup>((x) => x.ImageLinks), "image_links");
            MapAdGroup.Add(PropertyUtility.GetName<AdGroup>((x) => x.Name), "name");
            MapAdGroup.Add(PropertyUtility.GetName<AdGroup>((x) => x.Titles), "titles");
            MapAdGroup.Add(PropertyUtility.GetName<AdGroup>((x) => x.UpdateDate), "update_date");

            // Ad group with stats
            MapAdGroupWithStats.AddAll(MapAdGroup);
            MapAdGroupWithStats.Add(PropertyUtility.GetName<AdGroupWithStats>((x) => x.AdItemCount), "ad_item_count");

            // Ad item
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.Actions), "actions");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.AdGroupGuid), "ad_group_guid");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.AdGroupImageIndex), "ad_group_image_index");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.AdGroupTitleIndex), "ad_group_title_index");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.ApprovalState), "approval_state");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.CampaignGuid), "campaign_guid");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.Clicks), "clicks");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.Content), "content");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.Cpc), "cpc");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.CreateDate), "create_date");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.DeleteDate), "delete_date");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.Details), "details");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.Id), "id");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.ImageUrl), "image_url");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.Impressions), "impressions");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.ModifiedBeyondAdGroup), "modified_beyond_ad_group");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.SecondaryId), "secondary_id");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.Spent), "spent");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.Status), "status");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.Title), "title");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.UpdateDate), "update_date");
            MapAdItem.Add(PropertyUtility.GetName<AdItem>((x) => x.Url), "url");

            // Ad item with stats
            MapAdItemWithStats.AddAll(MapAdItem);
            //MapAdItemWithStats.Add(PropertyUtility.GetName<AdItemWithStats>((x) => x.), "");
        }

        /// <summary>
        /// Gets the database column name for a given property name.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="sortableProperty">Property name expression</param>
        /// <returns>Database column name</returns>
        internal static string Get<TEntity>(Expression<Func<TEntity, object>> sortableProperty)
            where TEntity : Entity
        {
            var type = typeof(TEntity);
            if (type == typeof(Account)) { return FromMap(sortableProperty, MapAccount); }
            if (type == typeof(CampaignWithStats)) { return FromMap(sortableProperty, MapCampaignWithStats); }
            if (type == typeof(Campaign)) { return FromMap(sortableProperty, MapCampaign); }
            if (type == typeof(CampaignGroupWithStats)) { return FromMap(sortableProperty, MapCampaignGroupWithStats); }
            if (type == typeof(CampaignGroup)) { return FromMap(sortableProperty, MapCampaignGroup); }
            if (type == typeof(AdItemWithStats)) { return FromMap(sortableProperty, MapAdItemWithStats); }
            if (type == typeof(AdItem)) { return FromMap(sortableProperty, MapAdItem); }
            if (type == typeof(AdGroupWithStats)) { return FromMap(sortableProperty, MapAdGroupWithStats); }
            if (type == typeof(AdGroup)) { return FromMap(sortableProperty, MapAdGroup); }

            throw new InvalidOperationException(nameof(TEntity));
        }

        internal static string Account(Expression<Func<Account, object>> sortableProperty) => FromMap(sortableProperty, MapAccount);
        internal static string Campaign(Expression<Func<Campaign, object>> sortableProperty) => FromMap(sortableProperty, MapCampaign);
        internal static string CampaignWithStats(Expression<Func<CampaignWithStats, object>> sortableProperty) => FromMap(sortableProperty, MapCampaignWithStats);
        internal static string CampaignGroup(Expression<Func<CampaignGroup, object>> sortableProperty) => FromMap(sortableProperty, MapCampaignGroup);
        internal static string CampaignGroupWithStats(Expression<Func<CampaignGroupWithStats, object>> sortableProperty) => FromMap(sortableProperty, MapCampaignGroupWithStats);
        internal static string AdItem(Expression<Func<AdItem, object>> sortableProperty) => FromMap(sortableProperty, MapAdItem);
        internal static string AdItemWithStats(Expression<Func<AdItemWithStats, object>> sortableProperty) => FromMap(sortableProperty, MapAdItemWithStats);
        internal static string AdGroup(Expression<Func<AdGroup, object>> sortableProperty) => FromMap(sortableProperty, MapAdGroup);
        internal static string AdGroupWithStats(Expression<Func<AdGroupWithStats, object>> sortableProperty) => FromMap(sortableProperty, MapAdGroupWithStats);

        /// <summary>
        /// Extract the value from the correct map.
        /// </summary>
        /// <param name="sortableProperty">Property name</param>
        /// <param name="map">The map to extract from</param>
        /// <returns></returns>
        private static string FromMap<TEntity>(Expression<Func<TEntity, object>> sortableProperty, Dictionary<string, string> map)
            where TEntity : Entity
        {
            if (sortableProperty == null) { throw new ArgumentNullException(nameof(sortableProperty)); }
            if (map == null) { throw new ArgumentNullException(nameof(map)); }

            var propertyName = PropertyUtility.GetName(sortableProperty);
            if (map.ContainsKey(propertyName))
            {
                return map[propertyName];
            }
            else
            {
                throw new InvalidOperationException(nameof(propertyName));
            }
        }

        /// <summary>
        /// Extension method to add one dictionary to the other.
        /// </summary>
        /// <param name="dictionary">Base</param>
        /// <param name="toAdd">Other</param>
        /// <returns><paramref name="dictionary"/></returns>
        public static Dictionary<string, string> AddAll(this Dictionary<string, string> dictionary, Dictionary<string, string> toAdd)
        {
            if (dictionary == null) { throw new ArgumentNullException(nameof(dictionary)); }
            if (toAdd == null) { throw new ArgumentNullException(nameof(toAdd)); }

            foreach (var key in toAdd.Keys)
            {
                dictionary.Add(key, toAdd[key]);
            }

            return dictionary;
        }

    }
}
