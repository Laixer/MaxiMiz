using Maximiz.Database.Columns;
using System;

namespace Maximiz.Database.Querying
{

    /// <summary>
    /// Base class for holding a search and order query.
    /// </summary>
    /// <typeparam name="TColumn">The type of column this query operates on</typeparam>
    public abstract class QueryBase<TColumn>
        where TColumn : Enum
    {

        /// <summary>
        /// The search query.
        /// Default value: <see cref="string.Empty"/>.
        /// </summary>
        public string SearchString { get; set; } = string.Empty;

        /// <summary>
        /// The column enum.
        /// TODO Not bulletproof.
        /// </summary>
        public abstract TColumn Column { get; set; }

        /// <summary>
        /// Determines the sorting order.
        /// Default value: <see cref="Order.Ascending"/>.
        /// </summary>
        public Order Order { get; set; } = Order.Ascending;

        /// <summary>
        /// Constructor for simplest creations of a query, including optional
        /// parameters for complex creation.
        /// </summary>
        /// <param name="searchQuery">The search query</param>
        /// <param name="column">The column to order by</param>
        /// <param name="order">The order</param>
        public QueryBase(string searchQuery, TColumn column, Order order = Order.Ascending)
        {
            SearchString = searchQuery ?? string.Empty;
            Column = column;
            Order = order;
        }

    }
}
