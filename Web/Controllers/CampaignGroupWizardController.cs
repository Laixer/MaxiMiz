using Microsoft.AspNetCore.Mvc;
using Maximiz.Model.Entity;
using Maximiz.ViewModels.CampaignGroupWizard;
using System.Threading.Tasks;
using System;
using Maximiz.ViewModels.Columns;
using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Mapper;
using Maximiz.ViewModels.EntityModels;
using Maximiz.QueryTranslation;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller to manage our <see cref="CampaignGroup"/> creation.
    /// </summary>
    public class CampaignGroupWizardController : Controller
    {

        private readonly IAdGroupWithStatsRepository _adGroupRepository;
        private readonly IMapper<AdGroupWithStats, AdGroupModel> _mapperAdGroup;
        private readonly IQueryTranslator _queryTranslator;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignGroupWizardController(IAdGroupWithStatsRepository adGroupRepository,
            IMapper<AdGroupWithStats, AdGroupModel> mapperAdGroup,
            IQueryTranslator queryTranslator)
        {
            _adGroupRepository = adGroupRepository ?? throw new ArgumentNullException(nameof(adGroupRepository));
            _mapperAdGroup = mapperAdGroup ?? throw new ArgumentNullException(nameof(mapperAdGroup));
            _queryTranslator = queryTranslator ?? throw new ArgumentNullException(nameof(queryTranslator));
        }

        /// <summary>
        /// Shows the campaign wizard.
        /// </summary>
        /// <returns><see cref="ViewResult"/></returns>
        [HttpGet]
        public IActionResult ShowWizard()
            => View("Wrapper");

        /// <summary>
        /// Submits the creation form, attempts to do the transactions and returns.
        /// </summary>
        /// <param name="model"><see cref="CampaignGroupFormAllViewModel"/></param>
        /// <returns><see cref="OkResult"/> or <see cref="BadRequestResult"/></returns>
        [HttpPost]
        public async Task<IActionResult> SubmitForm([FromBody] CampaignGroupFormAllViewModel model)
        {
            await Task.Delay(new Random().Next(250, 1000));

            // TODO Implement
            return Ok();
        }

        /// <summary>
        /// Get a partial view containing all ad groups based on some query.
        /// </summary>
        /// <param name="column"><see cref="ColumnCampaignGroupWizardAdGroup"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <param name="searchString">Search string</param>
        /// <param name="page">Page number</param>
        /// <returns><see cref="PartialViewResult"/></returns>
        public async Task<IActionResult> GetAdGroupsAsync(ColumnCampaignGroupWizardAdGroup column,
            Order order, string searchString = null, int page = 0)
        {
            if (page < 0) { throw new ArgumentOutOfRangeException(nameof(page)); }

            var query = _queryTranslator.Translate(column, order, searchString, page);
            return PartialView("_SectionAdGroupsTableRows", new CampaignGroupWizardAdGroupViewModel {
                AdGroupList = _mapperAdGroup.ConvertAll(await _adGroupRepository.GetAllAsync(query))
            });
        }

    }
}