using Maximiz.Database.Querying;
using Maximiz.Mapper;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using Maximiz.ViewModels.AdGroupOverview;
using Maximiz.ViewModels.EntityModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.ViewComponents.AdGroupOverview
{

    /// <summary>
    /// <see cref="ViewComponent"/> that manages loading a table that displays
    /// some queried result of <see cref="AdGroupModel"/>s async.
    /// TODO DRY
    /// </summary>
    public sealed class AdGroupOverviewTableViewComponent : ViewComponent
    {

        private readonly IAdGroupRepository _adGroupRepository;
        private readonly IMapper<AdGroupWithStats, AdGroupModel> _mapperAdGroup;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public AdGroupOverviewTableViewComponent(IAdGroupRepository adGroupRepository,
            IMapper<AdGroupWithStats, AdGroupModel> mapperAdGroup) 
        {
            _adGroupRepository = adGroupRepository;
            _mapperAdGroup = mapperAdGroup;
        }

        /// <summary>
        /// Invokes this view component and fetches a given query from the data
        /// store.
        /// TODO Who is going to translate the query object?
        /// </summary>
        /// <param name="table">The table type</param>
        /// <param name="query"><see cref="QueryCampaignWithStats"/></param>
        /// <returns>Partial view containing a table</returns>
        public async Task<IViewComponentResult> InvokeAsync(AdGroupOverviewTableType table, 
            QueryAdGroupWithStats query, int page = 0)
        {
            // Simulate waiting
            await Task.Delay(new Random().Next(25, 100));

            return View("TableRows", new AdGroupOverviewTableViewModel
            {
                AdGroupList = _mapperAdGroup.ConvertAll(
                    await GetAdGroupsByTable(table, query, page))
            });
        }

        /// <summary>
        /// Returns the (unconverted) campaigns that belong to a table, query and page.
        /// </summary>
        /// <param name="table"><see cref="AdGroupOverviewTableType"/></param>
        /// <param name="query"><see cref="QueryCampaignWithStats"/></param>
        /// <param name="page">Page number</param>
        /// <returns>Task that retreives the specified campaigns</returns>
        private Task<IEnumerable<AdGroupWithStats>> GetAdGroupsByTable(
            AdGroupOverviewTableType table, QueryAdGroupWithStats query, int page)
        {
            switch (table)
            {
                case AdGroupOverviewTableType.All:
                    return _adGroupRepository.GetAllAsync(query, page);
            }

            throw new InvalidOperationException(nameof(table));
        }

    }
}
