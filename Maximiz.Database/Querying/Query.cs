
namespace Maximiz.Database.Querying
{

    /// <summary>
    /// Base class for holding a search and order query.
    /// </summary>
    /// <typeparam name="TColumn">The type of column this query operates on</typeparam>
    public class Query<TColumn>
        where TColumn : System.Enum
    {

        /// <summary>
        /// The search query.
        /// Default value: <see cref="string.Empty"/>.
        /// </summary>
        public string SearchQuery { get; set; } = string.Empty;

        /// <summary>
        /// The column enum.
        /// TODO Not bulletproof.
        /// </summary>
        public TColumn Column { get; set; }

        /// <summary>
        /// Determines the sorting order.
        /// Default value: <see cref="Order.Descending"/>.
        /// </summary>
        public Order Order { get; set; } = Order.Ascending;

        /// <summary>
        /// Constructor for simplest creations of a query, including optional
        /// parameters for complex creation.
        /// </summary>
        /// <param name="column">The column to order by</param>
        /// <param name="order">The order</param>
        /// <param name="searchQuery">The search query</param>
        public Query(TColumn column, Order order = Order.Ascending, string searchQuery = "")
        {
            Column = column;
            Order = Order;
            SearchQuery = searchQuery;
        }

    }
}
