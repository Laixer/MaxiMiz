using Maximiz.ViewModels.CampaignOverview;
using Maximiz.ViewModels.Columns;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maximiz.Controllers.Abstraction
{

    /// <summary>
    /// Contract for our campaign overview controller.
    /// TODO Doc
    /// </summary>
    public interface ICampaignOverviewController
    {

        /// <summary>
        /// Loads the entire page as is.
        /// </summary>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpGet]
        IActionResult Overview();

        /// <summary>
        /// Gets our view component that gets table rows async.
        /// </summary>
        /// <param name="table">The table</param>
        /// <param name="query">The search query</param>
        /// <param name="column">The sorting column</param>
        /// <param name="order">The sorting order</param>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpGet]
        IActionResult GetCampaignTableViewComponent(CampaignTableType table,
            string query, ColumnCampaignOverview column, Order order);

        /// <summary>
        /// Gets our view component that gets campaign query count async.
        /// </summary>
        /// <param name="table">The table</param>
        /// <param name="query">The search query</param>
        /// <param name="column">The sorting column</param>
        /// <param name="order">The sorting order</param>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpGet]
        IActionResult GetCampaignCountViewComponent(CampaignTableType table,
            string query, ColumnCampaignOverview column, Order order);

        /// <summary>
        /// Deletes a campaign.
        /// </summary>
        /// <param name="campaignId">Internal campaign id</param>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpDelete]
        Task<IActionResult> Delete(Guid campaignId);

    }
}
