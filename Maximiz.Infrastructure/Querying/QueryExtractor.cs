using Maximiz.Core.Querying;
using Maximiz.Model.Entity;
using System;

namespace Maximiz.Infrastructure.Querying
{

    /// <summary>
    /// Contains our functionality to extract SQL queries.
    /// TODO This can be cleaned up, even though it works.
    /// </summary>
    internal static partial class QueryExtractor
    {

        /// <summary>
        /// Creates an SQL query based on some <paramref name="query"/>.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="query"><see cref="QueryBase{TEntity}"/></param>
        /// <returns>SQL query</returns>
        internal static string ExtractSql<TEntity>(QueryBase<TEntity> query, bool forCount = false)
            where TEntity : Entity
        {
            // TODO Throw if null? How to handle?

            var result = $"SELECT {(forCount ? "COUNT(*)" : "*")} FROM {GetTableName<TEntity>()}";
            result += (query == null) ? "" : $" {ExtractWhere(query, forCount)}";

            if (!forCount)
            {
                result += (query == null) ? "" : $" {ExtractSorting(query)}";
                result += (query == null) ? ExtractPaging() : $" {ExtractPaging(query)}";
            }

            result += ";";

            return result;
        }

        /// <summary>
        /// Creates an SQL query based on a <paramref name="page"/> and <paramref name="pageItemCount"/>
        /// in the case where we don't have a <see cref="QueryBase{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="page">Page number</param>
        /// <param name="pageItemCount">Items per page</param>
        /// <returns>SQL query</returns>
        internal static string ExtractSql<TEntity>(int page = 0, int pageItemCount = 50, bool forCount = false)
            where TEntity : Entity
        {
            if (page < 0) { throw new ArgumentOutOfRangeException(nameof(page)); }
            if (pageItemCount < 1) { throw new ArgumentOutOfRangeException(nameof(pageItemCount)); }

            var result = $"SELECT {(forCount ? "COUNT(*)" : "*")} FROM {GetTableName<TEntity>()}";

            if (!forCount)
            {
                result += $"{ExtractPaging(page, pageItemCount)}";
            }

            result += ";";

            return result;
        }

        internal static string ExtractSqlForSingle<TEntity>(PropertyEquality<TEntity> propertyEquality)
            where TEntity : Entity
        {
            if (propertyEquality == null) { throw new ArgumentNullException(nameof(propertyEquality)); }

            return $"SELECT * FROM {GetTableName<TEntity>()} WHERE {ExtractProperty(propertyEquality)} LIMIT 1;";
        }

    }
}
