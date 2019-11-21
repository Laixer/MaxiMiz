using Maximiz.Database.Querying;
using Maximiz.Mapper;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using Maximiz.ViewModels.CampaignGroupWizard;
using Maximiz.ViewModels.EntityModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maximiz.ViewComponents.CampaignGroupWizard
{

    /// <summary>
    /// <see cref="ViewComponent"/> for displaying our ad groups in the campaign group wizard.
    /// </summary>
    public sealed class CampaignGroupWizardAdGroupViewComponent : ViewComponent
    {

        private IAdGroupRepository _adGroupRepository;
        private IMapper<AdGroupWithStats, AdGroupModel> _mapper;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignGroupWizardAdGroupViewComponent(IAdGroupRepository adGroupRepository,
            IMapper<AdGroupWithStats, AdGroupModel> mapper)
        {
            _adGroupRepository = adGroupRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of ad groups based on a query. The listis displayed
        /// in a view and returned.
        /// </summary>
        /// <param name="query"><see cref="QueryAdGroupWithStats"/></param>
        /// <param name="page">Page number</param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IViewComponentResult> InvokeAsync(
            QueryAdGroupWithStats query, int page = 0)
        {
            // Simulate waiting
            await Task.Delay(new Random().Next(25, 100));

            return View("TableRows", new CampaignGroupWizardAdGroupViewModel
            {
                AdGroupList = _mapper.ConvertAll(
                    await _adGroupRepository.GetAllAsync(query, page))
            });
        }

    }
}
