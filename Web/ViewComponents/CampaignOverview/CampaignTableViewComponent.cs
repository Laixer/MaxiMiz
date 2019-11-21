using Maximiz.Database.Querying;
using Maximiz.Mapper;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using Maximiz.ViewModels.CampaignOverview;
using Maximiz.ViewModels.EntityModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.ViewComponents.CampaignOverview
{

    /// <summary>
    /// View component for loading our campaign tables async.
    /// </summary>
    public sealed class CampaignTableViewComponent : ViewComponent
    {

        /// <summary>
        /// Data store for campaigns.
        /// </summary>
        private readonly ICampaignRepository _campaignRepository;

        /// <summary>
        /// Maps our campaigns.
        /// </summary>
        private readonly IMapper<CampaignWithStats, CampaignModel> _mapperCampaign;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignTableViewComponent(ICampaignRepository campaignRepository,
            IMapper<CampaignWithStats, CampaignModel> mapperCampaign)
        {
            _campaignRepository = campaignRepository;
            _mapperCampaign = mapperCampaign;
        }

        /// <summary>
        /// Invokes this view component and fetches a given query from the data
        /// store.
        /// TODO Who is going to translate the query object?
        /// </summary>
        /// <param name="table">The table type</param>
        /// <param name="query"><see cref="QueryCampaignWithStats"/></param>
        /// <returns>Partial view containing a table</returns>
        public async Task<IViewComponentResult> InvokeAsync(CampaignTableType table, QueryCampaignWithStats query, int page = 0)
        {
            // Simulate waiting
            await Task.Delay(new Random().Next(25, 100));
            return View("TableRows", new CampaignOverviewTableViewModel
            {
                CampaignList = _mapperCampaign.ConvertAll(
                    await GetCampaignsByTable(table, query, page))
            });
        }

        /// <summary>
        /// Returns the (unconverted) campaigns that belong to a table, query and page.
        /// </summary>
        /// <param name="table"><see cref="CampaignTableType"/></param>
        /// <param name="query"><see cref="QueryCampaignWithStats"/></param>
        /// <param name="page">Page number</param>
        /// <returns>Task that retreives the specified campaigns</returns>
        private Task<IEnumerable<CampaignWithStats>> GetCampaignsByTable(CampaignTableType table, QueryCampaignWithStats query, int page)
        {
            switch (table)
            {
                case CampaignTableType.All:
                    return _campaignRepository.GetAllAsync(query, page);
                case CampaignTableType.Active:
                    return _campaignRepository.GetActiveAsync(query, page);
                case CampaignTableType.Inactive:
                    return _campaignRepository.GetInactiveAsync(query, page);
                case CampaignTableType.Pending:
                    return _campaignRepository.GetPendingAsync(query, page);
            }

            throw new InvalidOperationException(nameof(table));
        }

    }
}
