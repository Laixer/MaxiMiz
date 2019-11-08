using Maximiz.ViewModels.AdgroupWizard;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Maximiz.Controllers.Abstraction
{

    /// <summary>
    /// Interface for the new ad group controller.
    /// </summary>
    public interface IAdGroupWizardController
    {

        /// <summary>
        /// The start view where we select our complexity route.
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        IActionResult ShowWizard();

        /// <summary>
        /// Submits our form.
        /// </summary>
        /// <param name="model"><seealso cref="AdGroupFormViewModel"/></param>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpPost]
        Task<IActionResult> SubmitForm(AdGroupFormViewModel model);

        /// <summary>
        /// Returns a partial view with a title entry.
        /// </summary>
        /// <returns><see cref="IActionResult"</returns>
        [HttpGet]
        IActionResult GetPartialView();

    }
}
