using Maximiz.Database.Querying;
using Maximiz.Mapper.Enum;
using Maximiz.Repositories.Abstraction;
using Maximiz.ViewModels.AdGroupOverview;
using Maximiz.ViewModels.CampaignOverview;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maximiz.ViewComponents.AdGroupOverview
{

    /// <summary>
    /// <see cref="ViewComponent"/> that retrieves the amount of items existing
    /// in our data store for a given query async.
    /// </summary>
    public sealed class AdGroupOverviewCountViewComponent : ViewComponent
    {
        private readonly IAdGroupRepository _adGroupRepository;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public AdGroupOverviewCountViewComponent(IAdGroupRepository adGroupRepository)
        {
            _adGroupRepository = adGroupRepository;
        }

        /// <summary>
        /// Invokes this view component and fetches a given query from the data
        /// store.
        /// TODO Who is going to translate the query object?
        /// </summary>
        /// <param name="table">The table type</param>
        /// <param name="query"><see cref="QueryCampaignWithStats"/></param>
        /// <returns>Partial view containing a table</returns>
        public async Task<IViewComponentResult> InvokeAsync(AdGroupOverviewTableType table, QueryAdGroupWithStats query)
        {
            // Simulate waiting
            await Task.Delay(new Random().Next(25, 100));

            return View("TableCount", new AdGroupOverviewCountViewModel
            {
                TableName = MapperEnum.TranslateCampaignTableType(table),
                AdGroupCount = await GetAdGroupCount(table, query)
            });
        }

        /// <summary>
        /// Returns the campaign count that belong to a table, query and page.
        /// </summary>
        /// <param name="table"><see cref="CampaignTableType"/></param>
        /// <param name="query"><see cref="QueryCampaignWithStats"/></param>
        /// <param name="page">Page number</param>
        /// <returns>Task that retreives the campaign count</returns>
        private Task<int> GetAdGroupCount(AdGroupOverviewTableType table, QueryAdGroupWithStats query)
        {
            switch (table)
            {
                case AdGroupOverviewTableType.All:
                    return _adGroupRepository.GetCount(query);
            }

            throw new InvalidOperationException(nameof(table));
        }

    }
}

