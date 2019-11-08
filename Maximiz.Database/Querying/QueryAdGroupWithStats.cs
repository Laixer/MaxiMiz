using Maximiz.Database.Columns;

namespace Maximiz.Database.Querying
{

    /// <summary>
    /// Query for a <see cref="ColumnAdGroupWithStats"/> table.
    /// </summary>
    public sealed class QueryAdGroupWithStats : QueryBase<ColumnAdGroupWithStats>
    {

        /// <summary>
        /// The column with default value of <see cref="ColumnCampaignWithStats.Name"/>
        /// </summary>
        public override ColumnAdGroupWithStats Column { get; set; } = ColumnAdGroupWithStats.Name;

        /// <summary>
        /// Constructor for easy creation.
        /// </summary>
        /// <param name="column"><see cref="ColumnAdGroupWithStats"/></param>
        /// <param name="order"><see cref="Order"</param>
        /// <param name="searchQuery">Search string</param>
        public QueryAdGroupWithStats(string searchQuery = "", ColumnAdGroupWithStats column = ColumnAdGroupWithStats.Name, Order order = Order.Ascending)
            : base(searchQuery, column, order) { }

    }
}
