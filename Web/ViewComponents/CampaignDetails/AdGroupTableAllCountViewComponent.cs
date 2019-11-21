using Maximiz.Database.Querying;
using Maximiz.Repositories.Abstraction;
using Maximiz.ViewModels.CampaignDetails;
using Maximiz.ViewModels.CampaignOverview;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maximiz.ViewComponents.CampaignDetails
{

    /// <summary>
    /// View component for getting the total amount of ad groups for a given query.
    /// </summary>
    public class AdGroupTableAllCountViewComponent : ViewComponent
    {

        /// <summary>
        /// Data store for campaigns.
        /// </summary>
        private readonly IAdGroupRepository _adGroupRepository;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public AdGroupTableAllCountViewComponent(IAdGroupRepository adGroupRepository)
        {
            _adGroupRepository = adGroupRepository;
        }

        /// <summary>
        /// Invokes this view component and fetches a given query from the data
        /// store.
        /// TODO Who is going to translate the query object?
        /// </summary>
        /// <param name="query"><see cref="QueryAdGroupWithStats"/></param>
        /// <returns>Partial view containing a table</returns>
        public async Task<IViewComponentResult> InvokeAsync(QueryAdGroupWithStats query)
        {
            // Simulate waiting
            await Task.Delay(new Random().Next(25, 100));

            return View("TableCount", new AdGroupTableAllCountViewModel
            {
                TotalCount = await _adGroupRepository.GetCount(query)
            });
        }

    }
}
