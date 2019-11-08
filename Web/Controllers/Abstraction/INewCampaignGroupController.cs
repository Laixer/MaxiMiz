using Maximiz.ViewModels.NewCampaignGroup;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Maximiz.Controllers.Abstraction
{

    /// <summary>
    /// Contract for our new campaign group controller.
    /// </summary>
    public interface INewCampaignGroupController
    {

        /// <summary>
        /// Returns the main wrapper view for this controller.
        /// </summary>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpGet]
        IActionResult Index();

        /// <summary>
        /// Returns the basic or advanced choosing menu.
        /// </summary>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpGet]
        IActionResult ShowWizard();

        /// <summary>
        /// Submit the form for creating a new campaign group.
        /// </summary>
        /// <param name="model"><see cref="CampaignGroupFormAllViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        Task<IActionResult> SubmitForm(CampaignGroupFormAllViewModel model);

    }
}
