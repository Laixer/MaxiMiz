using static Maximiz.Infrastructure.Querying.QueryExtractor;
using static Maximiz.Infrastructure.Querying.DatabaseColumnMap;
using static Maximiz.Infrastructure.Querying.PropertyUtility;
using Maximiz.Model.Entity;
using System;
using System.Linq.Expressions;

namespace Maximiz.Infrastructure.Committing.Sql
{

    /// <summary>
    /// Contains private utility functions for unfolding statements.
    /// </summary>
    internal static partial class Sql
    {

        /// <summary>
        /// Returns an equality based on a property.
        /// </summary>
        /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
        /// <param name="property">The property we wish to evaluate</param>
        /// <returns>SQL equality bit</returns>
        private static string UpdateEquality<TEntity>(Expression<Func<TEntity, object>> property)
            where TEntity : Entity
            => $"{Get(property)} = @{GetName(property)}";

    }
}
