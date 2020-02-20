using Maximiz.ViewModels.AdGroupWizard;
using Microsoft.AspNetCore.Mvc;
using System;
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
        /// <returns><see cref="IActionResult"/></returns>
        [HttpGet]
        IActionResult ShowWizard();

        /// <summary>
        /// Shows the wizard as an editing panel, meaning all fields are already
        /// populated.
        /// </summary>
        /// <param name="adGroupId">Internal ad group id</param>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpGet]
        Task<IActionResult> ShowWizardAsEditor(Guid adGroupId);

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
        IActionResult GetTitleEntryPartialView();

    }
}
