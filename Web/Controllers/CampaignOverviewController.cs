using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Core.Querying;
using Maximiz.Mapper;
using Maximiz.Model.Entity;
using Maximiz.QueryTranslation;
using Maximiz.ViewModels.CampaignOverview;
using Maximiz.ViewModels.Columns;
using Maximiz.ViewModels.EntityModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Order = Maximiz.ViewModels.Columns.Order;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for our <see cref="CampaignModel"/> overview page.
    /// </summary>
    public sealed class CampaignOverviewController : Controller
    {

        private readonly IMapper<CampaignWithStats, CampaignModel> _mapperCampaign;
        private readonly ICampaignWithStatsRepository _campaignRepository;
        private readonly IQueryTranslator _queryTranslator;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignOverviewController(IMapper<CampaignWithStats, CampaignModel> mapperCampaign,
            ICampaignWithStatsRepository campaignRepository, IQueryTranslator queryTranslator)
        {
            _mapperCampaign = mapperCampaign ?? throw new ArgumentNullException(nameof(mapperCampaign));
            _campaignRepository = campaignRepository ?? throw new ArgumentNullException(nameof(campaignRepository));
            _queryTranslator = queryTranslator ?? throw new ArgumentNullException(nameof(queryTranslator));
        }

        /// <summary>
        /// Loads our campaign page.
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        public IActionResult Overview()
         => View("Overview", new CampaignOverviewViewModel
         {
             Column = ColumnCampaignOverview.Name,
             Order = Order.Ascending
         });

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
        public async Task<IActionResult> GetCampaignTableViewComponent(CampaignTableType table,
            ColumnCampaignOverview column, Order order, string searchString = null, int page = 1)
        {
            if (page < 1) { throw new ArgumentOutOfRangeException(nameof(page)); }

            var query = _queryTranslator.Translate(column, order, searchString, page);
            return PartialView("_TableRows", new CampaignOverviewTableViewModel
            {
                CampaignList = _mapperCampaign.ConvertAll(await GetCampaignsByTable(table, query))
            });
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
        public async Task<IActionResult> GetCampaignCountViewComponent(CampaignTableType table,
            ColumnCampaignOverview column, Order order, string searchString = null, int page = 1)
        {
            if (page < 1) { throw new ArgumentOutOfRangeException(nameof(page)); }

            var query = _queryTranslator.Translate(column, order, searchString, page);
            return PartialView("_TableCount", new CampaignCountViewModel
            {
                CampaignCount = await _campaignRepository.GetCountAsync(query)
            }); ;
        }

        /// <summary>
        /// Deletes a campaign.
        /// </summary>
        /// <param name="campaignId">Internal campaign id</param>
        /// <returns>View</returns>
        [HttpDelete]
        public Task<IActionResult> Delete(Guid campaignId)
        {
            if (campaignId == null || campaignId == Guid.Empty) { throw new ArgumentNullException(nameof(campaignId)); }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the (unconverted) campaigns that belong to a table, query and page.
        /// 
        /// TODO QUESTION Is having an external dependency in the function declaration 
        /// correct, even though it is private?
        /// </summary>
        /// <param name="table"><see cref="CampaignTableType"/></param>
        /// <param name="query"><see cref="QueryCampaignWithStats"/></param>
        /// <returns>Task that retreives the specified campaigns</returns>
        private Task<IEnumerable<CampaignWithStats>> GetCampaignsByTable(CampaignTableType table, QueryBase<CampaignWithStats> query)
        {
            switch (table)
            {
                case CampaignTableType.All:
                    return _campaignRepository.GetAllAsync(query);
                case CampaignTableType.Active:
                    return _campaignRepository.GetActiveAsync(query);
                case CampaignTableType.Inactive:
                    return _campaignRepository.GetInactiveAsync(query);
                case CampaignTableType.Pending:
                    return _campaignRepository.GetPendingAsync(query);
            }

            throw new InvalidOperationException(nameof(table));
        }

    }
}