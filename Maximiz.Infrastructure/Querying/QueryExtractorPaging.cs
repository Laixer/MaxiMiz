using Maximiz.Core.Querying;
using Maximiz.Model.Entity;
using System;

namespace Maximiz.Infrastructure.Querying
{

    /// <summary>
    /// Contains the functionality to extract the paging parameters for our
    /// SQL statements.
    /// </summary>
    internal static partial class QueryExtractor
    {

        /// <summary>
        /// Extracts the paging SQL clauses for our <paramref name="query"/>.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="query"><see cref="QueryBase{TEntity}"/></param>
        /// <returns>Paging SQL clause</returns>
        private static string ExtractPaging<TEntity>(QueryBase<TEntity> query)
            where TEntity : Entity
        {
            if (query == null) { return ExtractPaging(1, 50); }
            return ExtractPaging(query.Page, query.PageItemCount);
        }            

        /// <summary>
        /// Extracts the paging SQL clauses for our <paramref name="page"/> and
        /// our <paramref name="pageItemCount"/>.
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageItemCount">Items per page</param>
        /// <returns>Paging SQL clause</returns>
        private static string ExtractPaging(int page = 1, int pageItemCount = 50)
        {
            if (page < 1) { throw new ArgumentOutOfRangeException(nameof(page)); }
            if (pageItemCount < 1) { throw new ArgumentOutOfRangeException(nameof(pageItemCount)); }

            return $"OFFSET {(page - 1) * pageItemCount} LIMIT {pageItemCount}";
        }

    }
}
