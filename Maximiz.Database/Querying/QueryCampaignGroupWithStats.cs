using Maximiz.Database.Columns;

namespace Maximiz.Database.Querying
{

    /// <summary>
    /// Query for a <see cref="ColumnCampaignGroupWithStats"/> table.
    /// </summary>
    public sealed class QueryCampaignGroupWithStats : QueryBase<ColumnCampaignGroupWithStats>
    {

        /// <summary>
        /// The column with default value of <see cref="ColumnCampaignGroupWithStats.Name"/>
        /// </summary>
        public override ColumnCampaignGroupWithStats Column { get; set; } = ColumnCampaignGroupWithStats.Name;

        /// <summary>
        /// Constructor for easy creation.
        /// </summary>
        /// <param name="column"><see cref="ColumnCampaignGroupWithStats"/></param>
        /// <param name="order"><see cref="Order"</param>
        /// <param name="searchQuery">Search string</param>
        public QueryCampaignGroupWithStats(string searchQuery = "", ColumnCampaignGroupWithStats column = ColumnCampaignGroupWithStats.Name, Order order = Order.Ascending)
            : base(searchQuery, column, order) { }

    }
}
