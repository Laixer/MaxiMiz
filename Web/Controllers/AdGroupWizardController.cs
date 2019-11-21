using System.Threading.Tasks;
using Maximiz.Controllers.Abstraction;
using Maximiz.ViewModels.AdgroupWizard;
using Maximiz.ViewModels.CampaignDetails;
using Microsoft.AspNetCore.Mvc;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for creating ad groups.
    /// </summary>
    public class AdGroupWizardController : Controller, IAdGroupWizardController
    {

        /// <summary>
        /// The start view where we select our complexity route.
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        public IActionResult ShowWizard()
        {
            return View("Wrapper");
        }

        /// <summary>
        /// TODO Remove
        /// </summary>
        /// <returns>Debug view</returns>
        [HttpGet]
        public IActionResult Debug()
        {
            return View("Debug");
        }

        /// <summary>
        /// Submits our form.
        /// </summary>
        /// <param name="model"><see cref="AdGroupFormViewModel"</param>
        /// <returns><see cref="NoContentResult"/> or <see cref="BadRequestResult"/></returns>
        [HttpPost]
        public Task<IActionResult> SubmitForm([FromBody] AdGroupFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                return Task.FromResult<IActionResult>(NoContent());
            }
            else
            {
                return Task.FromResult<IActionResult>(BadRequest());
            }
        }

        [HttpGet]
        public IActionResult GetPartialView()
        {
            return PartialView("_TitleEntry", true);
        }

    }
}
