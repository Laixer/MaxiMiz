using System;
using System.Threading.Tasks;
using Maximiz.Controllers.Abstraction;
using Maximiz.Database.Querying;
using Maximiz.Mapper;
using Maximiz.Model.Entity;
using Maximiz.ViewModels.AdGroupOverview;
using Maximiz.ViewModels.Columns;
using Maximiz.ViewModels.Columns.Translation;
using Maximiz.ViewModels.EntityModels;
using Microsoft.AspNetCore.Mvc;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for the ad group overview tables.
    /// TODO Not for this version.
    /// </summary>
    public class AdGroupOverviewController : Controller, IAdGroupOverviewController
    {

        /// <summary>
        /// Converts our campaigns.
        /// </summary>
        private readonly IMapper<AdGroupWithStats, AdGroupModel> _mapperAdItems;


        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public AdGroupOverviewController(IMapper<AdGroupWithStats, AdGroupModel> mapperAdItems)
        {
            _mapperAdItems = mapperAdItems;
        }

        /// <summary>
        /// Loads our overview of adgroups.
        /// </summary>
        /// <returns><see cref="ViewResult"/></returns>
        [HttpGet]
        public IActionResult Overview()
        {
            return View("Overview", new AdGroupOverviewViewModel
            {
                Column = ColumnAdGroupOverview.Name,
                Order = Order.Ascending
            });
        }

        /// <summary>
        /// Gets our view component that gets table rows async.
        /// </summary>
        /// <param name="table"><see cref="AdGroupOverviewTableType"/></param>
        /// <param name="query">Search query string</param>
        /// <param name="column"><see cref="ColumnAdGroupOverview"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="ViewComponentResult"/></returns>
        [HttpGet]
        public IActionResult GetAdGroupTableViewComponent(AdGroupOverviewTableType table, 
            string query, ColumnAdGroupOverview column, Order order)
        {
            var columnDatabase = ColumnTranslator.Translate(column);
            var orderDatabase = OrderTranslator.Translate(order);
            var queryObject = new QueryAdGroupWithStats(query, columnDatabase, orderDatabase);
            return ViewComponent("AdGroupOverviewTable", new { table, query = queryObject });
        }

        /// <summary>
        /// Gets our view component that gets an ad group query count async.
        /// </summary>
        /// <param name="table"><see cref="AdGroupOverviewTableType"/></param>
        /// <param name="query">The search query string</param>
        /// <param name="column"><see cref="ColumnAdGroupOverview"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="ViewComponentResult"/></returns>
        [HttpGet]
        public IActionResult GetAdGroupCountViewComponent(AdGroupOverviewTableType table, 
            string query, ColumnAdGroupOverview column, Order order)
        {
            var columnDatabase = ColumnTranslator.Translate(column);
            var orderDatabase = OrderTranslator.Translate(order);
            var queryObject = new QueryAdGroupWithStats(query, columnDatabase, orderDatabase);
            return ViewComponent("AdGroupOverviewCount", new { table, query = queryObject });
        }

        /// <summary>
        /// Deletes an ad group.
        /// </summary>
        /// <param name="adGroupId">Internal ad group id</param>
        /// <returns><see cref="NoContentResult"/></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid adGroupId)
        {
            // Simulate waiting
            await Task.Delay(new Random().Next(200, 500));
            return NoContent();
        }

    }
}