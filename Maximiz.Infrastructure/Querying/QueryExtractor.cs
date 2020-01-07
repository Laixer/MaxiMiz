using Maximiz.Core.Querying;
using Maximiz.Model.Entity;
using System;

namespace Maximiz.Infrastructure.Querying
{

    /// <summary>
    /// Contains our functionality to extract SQL queries.
    /// </summary>
    internal static partial class QueryExtractor
    {

        /// <summary>
        /// Creates an SQL query based on some <paramref name="queryBase"/>.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="queryBase"><see cref="QueryBase{TEntity}"/></param>
        /// <returns>SQL query</returns>
        internal static string ExtractSql<TEntity>(QueryBase<TEntity> queryBase)
            where TEntity : Entity
        {
            if (queryBase == null) { throw new ArgumentNullException(nameof(queryBase)); }

            return $"SELECT * FROM {GetTableName<TEntity>()} " +
                $"{ExtractWhere(queryBase)} " +
                $"{ExtractSorting(queryBase)} " +
                $"{ExtractPaging(queryBase)};";
        }

        /// <summary>
        /// Creates an SQL query based on a <paramref name="page"/> and <paramref name="pageItemCount"/>
        /// in the case where we don't have a <see cref="QueryBase{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="page">Page number</param>
        /// <param name="pageItemCount">Items per page</param>
        /// <returns>SQL query</returns>
        internal static string ExtractSql<TEntity>(int page = 0, int pageItemCount = 50)
            where TEntity : Entity
        {
            if (page < 0) { throw new ArgumentOutOfRangeException(nameof(page)); }
            if (pageItemCount < 1) { throw new ArgumentOutOfRangeException(nameof(pageItemCount)); }

            return $"SELECT * FROM {GetTableName<TEntity>()} " +
                $"{ExtractPaging(page, pageItemCount)};";
        }

        /// <summary>
        /// Creates an SQL query based on some <paramref name="queryBase"/> to 
        /// get the total item count.
        /// </summary>
        /// 
        /// TODO DRY??
        /// 
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="queryBase"><see cref="QueryBase{TEntity}"/></param>
        /// <returns>SQL query for the total item count</returns>
        internal static string ExtractSqlForCount<TEntity>(QueryBase<TEntity> queryBase)
            where TEntity : Entity
        {
            if (queryBase == null) { throw new ArgumentNullException(nameof(queryBase)); }

            return $"SELECT COUNT(*) FROM {GetTableName<TEntity>()} " +
                $"{ExtractWhere(queryBase)} ;";
        }

        /// <summary>
        /// Creates an SQL query based on some to get the total item count.
        /// </summary>
        /// 
        /// TODO DRY??
        /// 
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <returns>SQL query for the total item count</returns>
        internal static string ExtractSqlForCount<TEntity>()
            where TEntity : Entity
            => $"SELECT COUNT(*) FROM {GetTableName<TEntity>()} ;";

    }
}
