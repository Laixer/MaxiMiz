using Maximiz.ViewModels.AdGroupOverview;
using Maximiz.ViewModels.Columns;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maximiz.Controllers.Abstraction
{

    /// <summary>
    /// Contract for our ad group overview controller.
    /// </summary>
    public interface IAdGroupOverviewController
    {

        /// <summary>
        /// Loads the overview.
        /// </summary>
        /// <returns><see cref="IActionResult"</returns>
        [HttpGet]
        IActionResult Overview();

        /// <summary>
        /// Gets our view component that gets table rows async.
        /// </summary>
        /// <param name="table"><see cref="AdGroupOverviewTableType"/></param>
        /// <param name="query">Search query string</param>
        /// <param name="column"><see cref="ColumnAdGroupOverview"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpGet]
        IActionResult GetAdGroupTableViewComponent(AdGroupOverviewTableType table,
            string query, ColumnAdGroupOverview column, Order order);

        /// <summary>
        /// Gets our view component that gets an ad group query count async.
        /// </summary>
        /// <param name="table"><see cref="AdGroupOverviewTableType"/></param>
        /// <param name="query">The search query string</param>
        /// <param name="column"><see cref="ColumnAdGroupOverview"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpGet]
        IActionResult GetAdGroupCountViewComponent(AdGroupOverviewTableType table,
            string query, ColumnAdGroupOverview column, Order order);

        /// <summary>
        /// Deletes an ad group.
        /// </summary>
        /// <param name="adGroupId">Internal ad group id</param>
        /// <returns><see cref="IActionResult"/></returns>
        [HttpDelete]
        Task<IActionResult> Delete(Guid adGroupId);

    }
}
