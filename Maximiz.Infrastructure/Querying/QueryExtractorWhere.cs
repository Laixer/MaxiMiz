using Maximiz.Core.Querying;
using Maximiz.Model.Entity;
using System;

namespace Maximiz.Infrastructure.Querying
{

    /// <summary>
    /// Contains functionality to translate a <see cref="QueryBase{TEntity}"/>
    /// to a correct SQL string.
    /// </summary>
    internal static partial class QueryExtractor
    {

        /// <summary>
        /// Extracts the WHERE clause for our SQL statement.
        /// </summary>
        /// <remarks>
        /// The result does not actually contain the WHERE keyword, so it can
        /// be chained with other WHERE statements.
        /// </remarks>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="query"><see cref="QueryBase{TEntity}"/></param>
        /// <returns>SQL WHERE clause<returns>
        internal static string ExtractWhere<TEntity>(QueryBase<TEntity> query)
            where TEntity : Entity
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            // Don't create a WHERE clause if we don't use a searchstring.
            if (string.IsNullOrEmpty(query.SearchString)) { return ""; }
            query.SearchString = ConvertSearchString(query.SearchString);

            switch (query)
            {
                case QueryBase<Account> x:
                    return ComposeWhereFromColumns(x.SearchString, new[] { "name" });
                case QueryBase<AdGroup> x:
                    return ComposeWhereFromColumns(x.SearchString, new[] { "name" });
                case QueryBase<AdGroupWithStats> x:
                    return ComposeWhereFromColumns(x.SearchString, new[] { "name" });
                case QueryBase<Campaign> x:
                    return ComposeWhereFromColumns(x.SearchString, new[] { "name", "branding_text" });
                case QueryBase<CampaignWithStats> x:
                    return ComposeWhereFromColumns(x.SearchString, new[] { "name", "branding_text" });
                case QueryBase<CampaignGroup> x:
                    return ComposeWhereFromColumns(x.SearchString, new[] { "name", "branding_text" });
                case QueryBase<CampaignGroupWithStats> x:
                    return ComposeWhereFromColumns(x.SearchString, new[] { "name", "branding_text" });
            }

            // TODO Which of these two do we want?
            // throw new InvalidOperationException(nameof(query));
            return "";
        }

        /// <summary>
        /// Creates the correct WHERE sql part based on the columns and searchstring.
        /// </summary>
        /// <param name="searchString">String to match against</param>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        private static string ComposeWhereFromColumns(string searchString, params string[] columnNames)
        {
            if (string.IsNullOrEmpty(searchString)) { throw new ArgumentNullException(nameof(searchString)); }
            if (columnNames == null) { throw new ArgumentNullException(nameof(columnNames)); }
            if (columnNames.Length == 0) { throw new InvalidOperationException(nameof(columnNames)); }

            var result = $"lower({columnNames[0]}) LIKE lower('{searchString}')";
            for (int i = 1; i < columnNames.Length; i++)
            {
                if (string.IsNullOrEmpty(columnNames[i])) { throw new ArgumentNullException(nameof(columnNames) +  $"[{i}]"); }
                result += $" OR lower({columnNames[i]}) LIKE lower('{searchString}')";
            }

            return result;
        }

        /// <summary>
        /// Prepares the <paramref name="searchString"/> for correct like statement
        /// formatting.
        /// </summary>
        /// <param name="searchString">The search string</param>
        /// <returns>The formatted <paramref name="searchString"/></returns>
        private static string ConvertSearchString(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) { throw new ArgumentNullException(nameof(searchString)); }
            if (searchString.Contains(";")) { throw new InvalidOperationException($"Search string should only contain letters and numbers"); }
            // TODO Make more secure! (Even though dapper forms its own security layer)

            return "searchString" + "%";
        }

    }
}
