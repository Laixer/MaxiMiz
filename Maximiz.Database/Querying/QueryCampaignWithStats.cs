using Maximiz.Database.Columns;

namespace Maximiz.Database.Querying
{

    /// <summary>
    /// Query for a <see cref="ColumnCampaignWithStats"/> table.
    /// </summary>
    public sealed class QueryCampaignWithStats : QueryBase<ColumnCampaignWithStats>
    {

        /// <summary>
        /// The column with default value of <see cref="ColumnCampaignWithStats.Name"/>
        /// </summary>
        public override ColumnCampaignWithStats Column { get; set; } = ColumnCampaignWithStats.Name;

        /// <summary>
        /// Constructor for easy creation.
        /// </summary>
        /// <param name="column"><see cref="ColumnCampaignWithStats"/></param>
        /// <param name="order"><see cref="Order"</param>
        /// <param name="searchString">Search string</param>
        public QueryCampaignWithStats(string searchString = "", ColumnCampaignWithStats column = ColumnCampaignWithStats.Name, Order order = Order.Ascending)
            : base(searchString, column, order) { }

    }
}
