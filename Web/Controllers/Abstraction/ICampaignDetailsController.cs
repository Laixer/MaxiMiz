using Maximiz.ViewModels.CampaignDetails;
using Maximiz.ViewModels.Columns;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maximiz.Controllers.Abstraction
{

    /// <summary>
    /// Contract for our campaign details controller.
    /// </summary>
    public interface ICampaignDetailsController
    {

        /// <summary>
        /// Displays the details for a single campaign.
        /// </summary>
        /// <param name="id">The database id of the campaign</param>
        /// <returns>Partial view</returns>
        Task<IActionResult> ShowCampaign(Guid id);

        /// <summary>
        /// Saves the user submitted form variables.
        /// </summary>
        /// <param name="model"><see cref="FormCampaignAccountViewModel"/></param>
        /// <returns>Action result</returns>
        [HttpPost]
        Task<IActionResult> PostFormAccount(FormCampaignAccountViewModel model);

        /// <summary>
        /// Saves the user submitted form variables.
        /// </summary>
        /// <param name="model"><see cref="FormCampaignMarketingViewModel"/></param>
        /// <returns>Action result</returns>
        [HttpPost]
        Task<IActionResult> PostFormMarketing(FormCampaignMarketingViewModel model);

        /// <summary>
        /// Saves the user submitted form variables.
        /// </summary>
        /// <param name="model"><see cref="FormCampaignMarketingViewModel"/></param>
        /// <returns>Action result</returns>
        [HttpPost]
        Task<IActionResult> PostFormPublishers(FormCampaignPublishersViewModel model);

        /// <summary>
        /// Returns the partial view containing all currently linked ad groups.
        /// </summary>
        /// <param name="campaignId">The id of the corresponding campaign</param>
        /// <returns>View</returns>
        [HttpGet]
        IActionResult GetAdGroupsLinkedViewComponent(Guid campaignId, string query, ColumnAdGroupLinking column, Order order);

        /// <summary>
        /// Returns the partial view containing all existing ad groups.
        /// </summary>
        /// <param name="campaignId">The id of the corresponding campaign</param>
        /// <returns>View</returns>
        [HttpGet]
        IActionResult GetAdGroupsAllViewComponent(Guid campaignId, string query, ColumnAdGroupLinking column, Order order);

        /// <summary>
        /// Links a given ad group to the campaign of the details view.
        /// </summary>
        /// <param name="model"><see cref="AdGroupConnectionViewModel"</param>
        /// <returns>No content actionresult</returns>
        [HttpPost]
        Task<IActionResult> LinkAdGroup(Guid campaignId, Guid adGroupId);

        /// <summary>
        /// Unlinks a given ad group to the campaign of the details view.
        /// </summary>
        /// <param name="model"><see cref="AdGroupConnectionViewModel"</param>
        /// <returns>No content actionresult</returns>
        [HttpPost]
        Task<IActionResult> UnlinkAdGroup(Guid campaignId, Guid adGroupId);

    }
}
