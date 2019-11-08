using Maximiz.Database.Columns;

namespace Maximiz.Database.Querying
{

    /// <summary>
    /// Query for a <see cref="ColumnAdItemWithStats"/> table.
    /// </summary>
    public sealed class QueryAdItemWithStats : QueryBase<ColumnAdItemWithStats>
    {

        /// <summary>
        /// The column with default value of <see cref="ColumnAdItemWithStats.Name"/>
        /// </summary>
        public override ColumnAdItemWithStats Column { get; set; } = ColumnAdItemWithStats.Name;

        /// <summary>
        /// Constructor for easy creation.
        /// </summary>
        /// <param name="column"><see cref="ColumnAdItemWithStats"/></param>
        /// <param name="order"><see cref="Order"</param>
        /// <param name="searchQuery">Search string</param>
        public QueryAdItemWithStats(string searchQuery = "", ColumnAdItemWithStats column = ColumnAdItemWithStats.Name, Order order = Order.Ascending)
            : base(searchQuery, column, order) { }

    }
}
