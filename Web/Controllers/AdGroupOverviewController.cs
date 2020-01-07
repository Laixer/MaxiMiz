using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Mapper;
using Maximiz.Model.Entity;
using Maximiz.QueryTranslation;
using Maximiz.ViewModels.AdGroupOverview;
using Maximiz.ViewModels.Columns;
using Maximiz.ViewModels.EntityModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for the ad group overview tables.
    /// </summary>
    public class AdGroupOverviewController : Controller
    {

        private readonly IAdGroupWithStatsRepository _adGroupWithStatsRepository;
        private readonly IMapper<AdGroupWithStats, AdGroupModel> _mapperAdGroupWithStats;
        private readonly IQueryTranslator _queryTranslator;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public AdGroupOverviewController(IAdGroupWithStatsRepository adGroupWithStatsRepository,
            IMapper<AdGroupWithStats, AdGroupModel> mapperAdGroupWithStats,
            IQueryTranslator queryTranslator)
        {
            _adGroupWithStatsRepository = adGroupWithStatsRepository ?? throw new ArgumentNullException(nameof(adGroupWithStatsRepository));
            _mapperAdGroupWithStats = mapperAdGroupWithStats ?? throw new ArgumentNullException(nameof(mapperAdGroupWithStats));
            _queryTranslator = queryTranslator ?? throw new ArgumentNullException(nameof(queryTranslator));
        }

        /// <summary>
        /// Loads our overview of adgroups.
        /// </summary>
        /// <returns><see cref="ViewResult"/></returns>
        [HttpGet]
        public IActionResult Overview()
            => View("Overview", new AdGroupOverviewViewModel
            {
                Column = ColumnAdGroupOverview.Name,
                Order = Order.Ascending
            });

        /// <summary>
        /// Retrieves our requested <see cref="AdGroupWithStats"/> in a partial
        /// view containing a table row for each item.
        /// </summary>
        /// <remarks>
        /// TODO Implement table switch functionality in the future
        /// </remarks>
        /// <param name="table"><see cref="AdGroupOverviewTableType"/></param>
        /// <param name="column"><see cref="ColumnAdGroupOverview"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <param name="searchString">Search query string</param>
        /// <param name="page">The page to display</param>
        /// <returns><see cref="PartialViewResult"/></returns>
        [HttpGet]
        public async Task<IActionResult> GetAdGroupTableAsync(AdGroupOverviewTableType table,
            ColumnAdGroupOverview column, Order order, string searchString = null, int page = 0)
        {
            // Only edge care required
            if (page < 0) { throw new ArgumentOutOfRangeException(nameof(page)); }

            var query = _queryTranslator.Translate(column, order, searchString, page);
            return PartialView("_TableRows", new AdGroupOverviewTableViewModel
            {
                AdGroupList = _mapperAdGroupWithStats.ConvertAll(await _adGroupWithStatsRepository.GetAllAsync(query))
            });
        }

        /// <summary>
        /// Gets the item count for a given query in our data store and returns
        /// it in its own partial view.
        /// </summary>
        /// <remarks>
        /// TODO Implement table switch
        /// </remarks>
        /// <param name="table"><see cref="AdGroupOverviewTableType"/></param>
        /// <param name="column"><see cref="ColumnAdGroupOverview"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <param name="searchString">The search query string</param>
        /// <param name="page">The page to display</param>
        /// <returns><see cref="PartialViewResult"/></returns>
        [HttpGet]
        public async Task<IActionResult> GetAdGroupCountAsync(AdGroupOverviewTableType table,
            ColumnAdGroupOverview column, Order order, string searchString = null, int page = 0)
        {
            // Only edge care required
            if (page < 0) { throw new ArgumentOutOfRangeException(nameof(page)); }

            var query = _queryTranslator.Translate(column, order, searchString, page);
            return PartialView("_TableRows", new AdGroupOverviewCountViewModel
            {
                AdGroupCount = await _adGroupWithStatsRepository.GetCountAsync(query)
            });
        }

        /// <summary>
        /// Deletes an ad group.
        /// </summary>
        /// <param name="adGroupId">Internal ad group id</param>
        /// <returns><see cref="OkResult"/> or <see cref="BadRequestResult"/></returns>
        [HttpDelete]
        public Task<IActionResult> Delete(Guid adGroupId)
        {
            if (adGroupId == null || adGroupId == Guid.Empty) { throw new ArgumentNullException(nameof(adGroupId)); }

            throw new NotImplementedException();
        }

    }
}