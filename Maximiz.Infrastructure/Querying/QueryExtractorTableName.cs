using Maximiz.Model.Entity;
using System;

namespace Maximiz.Infrastructure.Querying
{

    /// <summary>
    /// Contains the functionality to extract the table name.
    /// </summary>
    internal static partial class QueryExtractor
    {

        private static readonly string SqlDomain = "public";

        /// <summary>
        /// Gets the table name and domain for a given type of <see cref="Entity"/>.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <returns>Domain + table name as a string</returns>
        internal static string GetTableName<TEntity>()
            where TEntity : Entity 
            => $"{SqlDomain}.{TableName<TEntity>()}";

        /// <summary>
        /// This is a beunfix, make this cleaner.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <returns>Table name as a string</returns>
        private static string TableName<TEntity>()
            where TEntity : Entity
        {
            var type = typeof(TEntity);
            if (type == typeof(Account)) { return "account"; }
            if (type == typeof(CampaignWithStats)) { return "campaign_with_stats"; }
            if (type == typeof(Campaign)) { return "campaign"; }
            if (type == typeof(CampaignGroupWithStats)) { return "campaign_group_with_stats"; }
            if (type == typeof(CampaignGroup)) { return "campaign_group"; }
            if (type == typeof(AdItemWithStats)) { return "ad_item_with_stats"; }
            if (type == typeof(AdItem)) { return "ad_item"; }
            if (type == typeof(AdGroupWithStats)) { return "ad_group_with_stats"; }
            if (type == typeof(AdGroup)) { return "ad_group"; }

            throw new InvalidOperationException(nameof(TEntity));
        }
    }
}
