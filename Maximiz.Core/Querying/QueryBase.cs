using Maximiz.Model.Entity;
using System;
using System.Linq.Expressions;

namespace Maximiz.Core.Querying
{

    /// <summary>
    /// Query base for a search query in our data store.
    /// </summary>
    /// <typeparam name="TEntity"><see cref="Entity"/></typeparam>
    public abstract class QueryBase<TEntity>
        where TEntity : Entity
    {

        /// <summary>
        /// The page number to display.
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        /// The items per page to display.
        /// </summary>
        public int PageItemCount { get; set; } = 50;

        /// <summary>
        /// The string to search for.
        /// </summary>
        public string SearchString { get; set; } = "";

        /// <summary>
        /// The returned sorting order.
        /// </summary>
        public Order Order { get; set; } = Order.Ascending;

        /// <summary>
        /// Property of our <see cref="TEntity"/> based on which we sort.
        /// </summary>
        public Expression<Func<TEntity>> SortableProperty { get; set; }

    }
}
