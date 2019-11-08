using Microsoft.AspNetCore.Mvc;
using Maximiz.Model.Entity;
using Maximiz.Controllers.Abstraction;
using Maximiz.ViewModels.NewCampaignGroup;
using System.Threading.Tasks;
using System;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller to manage our <see cref="CampaignGroup"/> creation.
    /// </summary>
    public class NewCampaignGroupController : Controller, INewCampaignGroupController
    {

        /// <summary>
        /// Returns the main wrapper view for this controller.
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View("Wrapper");
        }

        /// <summary>
        /// Allows us to choose between the different routes.
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public IActionResult ShowWizard()
        {
            return View("Wrapper");
        }

        /// <summary>
        /// Submits the creation form, attempts to do the transactions and returns.
        /// </summary>
        /// <param name="model"><see cref="CampaignGroupFormAllViewModel"/></param>
        /// <returns>No content</returns>
        [HttpPost]
        public async Task<IActionResult> SubmitForm(CampaignGroupFormAllViewModel model)
        {
            await Task.Delay(new Random().Next(250, 1000));

            // Check modelstate.valid
            // Do transaction

            return NoContent();
        }

    }
}