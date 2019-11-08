using Maximiz.Database.Querying;
using Maximiz.Mapper.Enum;
using Maximiz.Repositories.Abstraction;
using Maximiz.ViewModels.CampaignOverview;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maximiz.ViewComponents.CampaignOverview
{

    /// <summary>
    /// View component for getting the total amount of campaigns for any given
    /// query.
    /// </summary>
    public class CampaignCountViewComponent : ViewComponent
    {

        /// <summary>
        /// Data store for campaigns.
        /// </summary>
        private readonly ICampaignRepository _campaignRepository;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignCountViewComponent(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }

        /// <summary>
        /// Invokes this view component and fetches a given query from the data
        /// store.
        /// TODO Who is going to translate the query object?
        /// </summary>
        /// <param name="table">The table type</param>
        /// <param name="query"><see cref="QueryCampaignWithStats"/></param>
        /// <returns>Partial view containing a table</returns>
        public async Task<IViewComponentResult> InvokeAsync(CampaignTableType table, QueryCampaignWithStats query)
        {
            // Simulate waiting
            await Task.Delay(new Random().Next(25, 100));

            return View("TableCount", new CampaignCountViewModel
            {
                TableName = MapperEnum.TranslateCampaignTableType(table),
                CampaignCount = await GetCampaignCount(table, query)
            });
        }

        /// <summary>
        /// Returns the campaign count that belong to a table, query and page.
        /// </summary>
        /// <param name="table"><see cref="CampaignTableType"/></param>
        /// <param name="query"><see cref="QueryCampaignWithStats"/></param>
        /// <param name="page">Page number</param>
        /// <returns>Task that retreives the campaign count</returns>
        private Task<int> GetCampaignCount(CampaignTableType table, QueryCampaignWithStats query)
        {
            switch (table)
            {
                case CampaignTableType.All:
                    return _campaignRepository.GetCount(query);
                case CampaignTableType.Active:
                    // TODO Add active to search string
                    return _campaignRepository.GetCount(query);
                case CampaignTableType.Inactive:
                    // TODO Add inactive to search string
                    return _campaignRepository.GetCount(query);
                case CampaignTableType.Pending:
                    // TODO Add pending to search string
                    return _campaignRepository.GetCount(query);
            }

            throw new InvalidOperationException(nameof(table));
        }

    }
}
