using Maximiz.Core.Querying;
using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Maximiz.Infrastructure.Querying
{

    /// <summary>
    /// Contains functionality to translate a <see cref="QueryBase{TEntity}"/>
    /// to a correct SQL string.
    /// </summary>
    internal static partial class QueryExtractor
    {

        /// <summary>
        /// Extracts the WHERE clause for our SQL statement based on a
        /// <see cref="QueryBase{TEntity}"/>.
        /// </summary>
        /// <remarks>
        /// The result does not actually contain the WHERE keyword, so it can
        /// be chained with other WHERE statements.
        /// </remarks>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="query"><see cref="QueryBase{TEntity}"/></param>
        /// <returns>SQL WHERE clause<returns>
        private static string ExtractWhere<TEntity>(QueryBase<TEntity> query, bool forCount = false)
            where TEntity : Entity
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            // TODO Seems bug-sensitive
            var equalities = new List<string>();
            if (!string.IsNullOrEmpty(query.SearchString)) { equalities.Add(ExtractSearch(query)); }
            if (query.PropertyEquality != null) { equalities.Add(ExtractProperty(query.PropertyEquality)); }

            // Construct
            if (equalities.Count == 0) { return ""; }
            if (equalities.Count == 1) { return $"WHERE {equalities[0]}"; }
            else
            {
                var result = $"WHERE {equalities[0]}";
                for (int i = 1; i < equalities.Count; i++)
                {
                    result += $" AND {equalities[i]}";
                }
                return result;
            }
        }

        /// <summary>
        /// Extracts the WHERE clause for our SQL statement based on a 
        /// <see cref="PropertyEquality{TEntity, TValue}"/>.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <typeparam name="TValue">The property values</typeparam>
        /// <param name="propertyEquality"><see cref="PropertyEquality{TEntity, TValue}"/></param>
        /// <returns>SQL WHERE clause</returns>
        private static string ExtractWhere<TEntity, TValue>(PropertyEquality<TEntity> propertyEquality)
            where TEntity : Entity
        {
            if (propertyEquality == null) { throw new ArgumentNullException(nameof(propertyEquality)); }
            return $"WHERE {ExtractProperty(propertyEquality)}";
        }

        /// <summary>
        /// Extracts the <see cref="QueryBase{TEntity}.SearchString"/> equality.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="query"><see cref="QueryBase{TEntity}"/></param>
        /// <returns>SQL equality for searchstring<returns>
        private static string ExtractSearch<TEntity>(QueryBase<TEntity> query)
            where TEntity : Entity
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            if (string.IsNullOrEmpty(query.SearchString)) { throw new ArgumentNullException(nameof(query.SearchString)); }

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

            throw new InvalidOperationException(nameof(query));
        }

        /// <summary>
        /// Creates the correct WHERE sql part based on the columns and searchstring.
        /// </summary>
        /// <remarks>
        /// This adds parentheses to the equality in case of multiple OR statements.
        /// </remarks>
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
                if (string.IsNullOrEmpty(columnNames[i])) { throw new ArgumentNullException(nameof(columnNames) + $"[{i}]"); }
                result += $" OR lower({columnNames[i]}) LIKE lower('{searchString}')";
            }

            if (columnNames.Length > 1)
            {
                return $"({result})";
            }
            else
            {
                return result;
            }
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

            return $"{searchString}%";
        }

        /// <summary>
        /// Resolves a <see cref="PropertyEquality{TEntity, TValue}"/> into 
        /// the corresponding SQL equality.
        /// </summary>
        /// <remarks>
        /// This includes parentheses in the result if the value list is longer
        /// than one.
        /// </remarks>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="input"><see cref="PropertyEquality{TEntity}"/></param>
        /// <returns>SQL equality</returns>
        private static string ExtractProperty<TEntity>(PropertyEquality<TEntity> input)
            where TEntity : Entity
        {
            if (input.Property == null) { throw new ArgumentNullException(nameof(input.Property)); }

            // TODO Additional checks? Actually flawed by design?

            var columnName = DatabaseColumnMap.Get(input.Property);
            var strings = input.UseTranslation();

            var result = $"{columnName} = '{strings[0]}'";
            for (int i = 1; i < strings.Count; i++)
            {
                result += $" OR {columnName} = '{strings[i]}'";
            }

            if (strings.Count > 1)
            {
                return $"({result})";
            }
            else
            {
                return result;
            }
        }

    }
}
