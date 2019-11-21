
namespace Maximiz.ViewModels.CampaignOverview
{

    /// <summary>
    /// View model for displaying the total count of ad groups.
    /// </summary>
    public sealed class AdGroupOverviewCountViewModel
    {

        /// <summary>
        /// Indicates the name of the table.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Indicates the total amount of items found by some query.
        /// </summary>
        public int AdGroupCount { get; set; }

    }
}
