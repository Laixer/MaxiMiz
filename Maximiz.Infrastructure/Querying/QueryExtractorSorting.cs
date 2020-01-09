using Maximiz.Core.Querying;
using Maximiz.Model.Entity;
using System;

namespace Maximiz.Infrastructure.Querying
{

    /// <summary>
    /// Contains functionality to extract the sorting SQL clauses from a query.
    /// </summary>
    internal static partial class QueryExtractor
    {

        /// <summary>
        /// Extracts our sorting SQL clause from our <paramref name="query"/>.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="query"><see cref="QueryBase{TEntity}"/></param>
        /// <returns>Sorting SQL clause</returns>
        private static string ExtractSorting<TEntity>(QueryBase<TEntity> query)
            where TEntity : Entity
        {
            if (query == null) { return ""; } // TODO Should we do this?
            if (query.SortableProperty == null) { throw new ArgumentNullException(nameof(query.SortableProperty)); }

            var columnName = DatabaseColumnMap.Get(query.SortableProperty);
            var direction = OrderTranslator.Translate(query.Order);
            return $"ORDER BY {columnName} {direction}";
        }

    }

}
