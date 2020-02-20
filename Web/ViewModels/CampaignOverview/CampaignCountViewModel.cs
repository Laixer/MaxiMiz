
namespace Maximiz.ViewModels.CampaignOverview
{

    /// <summary>
    /// View model for displaying the total count of campaigns.
    /// </summary>
    public sealed class CampaignCountViewModel
    {

        /// <summary>
        /// Indicates the name of the table.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Indicates the total amount of items found by some query.
        /// </summary>
        public int CampaignCount { get; set; }

        /// <summary>
        /// Indicates the total amount of items for each page.
        /// </summary>
        public int ItemsPerPage { get; set; }

    }
}
