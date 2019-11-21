using Maximiz.Controllers.Abstraction;
using Maximiz.Database.Querying;
using Maximiz.Mapper;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using Maximiz.Transactions;
using Maximiz.ViewModels.CampaignOverview;
using Maximiz.ViewModels.Columns;
using Maximiz.ViewModels.Columns.Translation;
using Maximiz.ViewModels.EntityModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for requests related to campaigns.
    /// </summary>
    public sealed class CampaignOverviewController : Controller, ICampaignOverviewController
    {

        /// <summary>
        /// Converts our campaigns.
        /// </summary>
        private readonly IMapper<CampaignWithStats, CampaignModel> _mapperCampaign;

        /// <summary>
        /// Manages entity transactions for us.
        /// </summary>
        private readonly ITransactionHandler _transactionHandler;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="mapperCampaign">Mapper from external to viewmodel</param>
        /// <param name="transactionHandler">The transaction handler</param>
        public CampaignOverviewController(IMapper<CampaignWithStats, CampaignModel> mapperCampaign,
            ITransactionHandler transactionHandler)
        {
            _mapperCampaign = mapperCampaign;
            _transactionHandler = transactionHandler;
        }

        /// <summary>
        /// Loads our campaign page.
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        public IActionResult Overview()
        {
            return View("Overview", new CampaignOverviewViewModel
            {
                Column = ColumnCampaignOverview.Name,
                Order = Order.Ascending
            });
        }

        /// <summary>
        /// Gets our view component that gets table rows async.
        /// TODO This translates, is that correct? Responsibility?
        /// </summary>
        /// <param name="table">The table</param>
        /// <param name="query">The search query</param>
        /// <param name="column">The sorting column</param>
        /// <param name="order">The sorting order</param>
        /// <returns>View component call</returns>
        [HttpGet]
        public IActionResult GetCampaignTableViewComponent(CampaignTableType table,
            string query, ColumnCampaignOverview column, Order order)
        {
            var columnDatabase = ColumnTranslator.Translate(column);
            var orderDatabase = OrderTranslator.Translate(order);
            var queryObject = new QueryCampaignWithStats(query, columnDatabase, orderDatabase);
            return ViewComponent("CampaignTable", new { table, query = queryObject });
        }


        /// <summary>
        /// Gets our view component that gets campaign query count async.
        /// </summary>
        /// <param name="table">The table</param>
        /// <param name="query">The search query</param>
        /// <param name="column">The sorting column</param>
        /// <param name="order">The sorting order</param>
        /// <returns>View component call</returns>
        [HttpGet]
        public IActionResult GetCampaignCountViewComponent(CampaignTableType table,
            string query, ColumnCampaignOverview column, Order order)
        {
            var columnDatabase = ColumnTranslator.Translate(column);
            var orderDatabase = OrderTranslator.Translate(order);
            var queryObject = new QueryCampaignWithStats(query, columnDatabase, orderDatabase);
            return ViewComponent("CampaignCount", new { table, query = queryObject });
        }

        /// <summary>
        /// Deletes a campaign.
        /// </summary>
        /// <param name="campaignId">Internal campaign id</param>
        /// <returns>View</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid campaignId)
        {
            // Simulate waiting
            await Task.Delay(new Random().Next(200, 500));
            return NoContent();
        }

    }
}