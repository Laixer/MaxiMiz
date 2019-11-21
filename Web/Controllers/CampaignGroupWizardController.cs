using Microsoft.AspNetCore.Mvc;
using Maximiz.Model.Entity;
using Maximiz.Controllers.Abstraction;
using Maximiz.ViewModels.CampaignGroupWizard;
using System.Threading.Tasks;
using System;
using Maximiz.ViewModels.Columns;
using Maximiz.ViewModels.Columns.Translation;
using Maximiz.Database.Querying;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller to manage our <see cref="CampaignGroup"/> creation.
    /// </summary>
    public class CampaignGroupWizardController : Controller, ICampaignGroupWizardController
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
        public async Task<IActionResult> SubmitForm([FromBody] CampaignGroupFormAllViewModel model)
        {
            await Task.Delay(new Random().Next(250, 1000));

            // Check modelstate.valid
            // Do transaction

            // TODO Debug remove
            return BadRequest();
            return NoContent();
        }

        /// <summary>
        /// Gets our view component that retrieves ad groups in list form.
        /// </summary>
        /// <param name="query">Search query string</param>
        /// <param name="column"><see cref="column"/></param>
        /// <param name="order"></param>
        /// <returns><see cref="ViewComponent"/></returns>
        [HttpGet]
        public IActionResult GetAdGroupsViewComponent(string query, ColumnCampaignGroupWizardAdGroup column, Order order)
        {
            var columnDatabase = ColumnTranslator.Translate(column);
            var orderDatabase = OrderTranslator.Translate(order);
            var queryObject = new QueryAdGroupWithStats(query, columnDatabase, orderDatabase);
            return ViewComponent("CampaignGroupWizardAdGroup", new { query = queryObject });
        }

    }
}