using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Mapper;
using Maximiz.Model.Entity;
using Maximiz.QueryTranslation;
using Maximiz.ViewModels.CampaignDetails;
using Maximiz.ViewModels.Columns;
using Maximiz.ViewModels.EntityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for showing and editing campaign details.
    /// </summary>
    public sealed class CampaignDetailsController : Controller
    {

        private readonly ICampaignWithStatsRepository _campaignRepository;
        private readonly IAdGroupWithStatsRepository _adGroupRepository;
        private readonly IAccountWithStatsRepository _accountRepository;

        private readonly IMapper<CampaignWithStats, CampaignModel> _mapperCampaign;
        private readonly IMapper<AdGroupWithStats, AdGroupModel> _mapperAdGroup;
        private readonly IMapper<AccountWithStats, AccountModel> _mapperAccount;
        private readonly IQueryTranslator _queryTranslator;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignDetailsController(ICampaignWithStatsRepository campaignRepository,
            IAdGroupWithStatsRepository adGroupRepository,
            IAccountWithStatsRepository accountRepository,
            IMapper<CampaignWithStats, CampaignModel> mapperCampaign,
            IMapper<AdGroupWithStats, AdGroupModel> mapperAdGroup,
            IMapper<AccountWithStats, AccountModel> mapperAccount,
            IQueryTranslator queryTranslator,
            ILoggerFactory loggerFactory)
        {
            _campaignRepository = campaignRepository ?? throw new ArgumentNullException(nameof(campaignRepository));
            _adGroupRepository = adGroupRepository ?? throw new ArgumentNullException(nameof(adGroupRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

            _mapperCampaign = mapperCampaign ?? throw new ArgumentNullException(nameof(mapperCampaign));
            _mapperAdGroup = mapperAdGroup ?? throw new ArgumentNullException(nameof(mapperAdGroup));
            _mapperAccount = mapperAccount ?? throw new ArgumentNullException(nameof(mapperAccount));

            _queryTranslator = queryTranslator ?? throw new ArgumentNullException(nameof(queryTranslator));

            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            logger = loggerFactory.CreateLogger(nameof(CampaignDetailsController));
        }

        /// <summary>
        /// Instantiates the details view for a <see cref="CampaignWithStats"/>
        /// with a specified id.
        /// </summary>
        /// <param name="id">The campaign id</param>
        /// <returns>View</returns>
        [HttpGet]
        public async Task<IActionResult> ShowCampaign(Guid id)
        {
            // First get campaign
            if (id == null || id == Guid.Empty) { throw new ArgumentNullException(nameof(id)); }
            var campaign = _mapperCampaign.Convert(await _campaignRepository.GetAsync(id));

            // Then get account
            if (campaign.AccountGuid == null || campaign.AccountGuid == Guid.Empty) { throw new ArgumentNullException(nameof(campaign.AccountGuid)); }
            var account = _mapperAccount.Convert(await _accountRepository.GetAsync(campaign.AccountGuid));

            return PartialView("_Details", new CampaignDetailsViewModel
            {
                Campaign = campaign,
                Account = account
            });
        }

        /// <summary>
        /// Saves the user submitted form variables.
        /// </summary>
        /// <param name="model"><see cref="FormCampaignDetailsViewModel"/></param>
        /// <returns><see cref="OkResult"/></returns>
        [HttpPost]
        public async Task<IActionResult> PostModificationForm([FromBody] FormCampaignDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //try
            //{
            //    var entityMap = await _entityExtractor.Extract(model);
            //    if (await _stateManager.AttemptStartStateMachineAsync(entityMap))
            //    {
            //        return Ok();
            //    }
            //    else
            //    {
            //        // TODO Is this the right way to do this?
            //        // TODO Implement more specific feedback?
            //        return BadRequest();
            //    }
            //} catch (Exception e)
            //{
            //    logger.LogError(e, $"Error while attempting to launch campaign details operation");
            //    return BadRequest();
            //}

            // Simulate waiting
            await Task.Delay(1000);

            return Ok();
        }

        /// <summary>
        /// Returns a partial view containing ad groups that are linked to a 
        /// given campaign.
        /// </summary>
        /// <param name="campaignId">Internal campaign <see cref="Guid"/></param>
        /// <param name="column"><see cref="ColumnAdGroupLinking"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <param name="searchString">Search string</param>
        /// <param name="page">Page</param>
        /// <returns><see cref="PartialViewResult"/></returns>
        [HttpGet]
        public async Task<IActionResult> GetAdGroupsLinkedAsync(Guid campaignId, ColumnAdGroupLinking column,
            Order order, string searchString = null, int page = 1)
        {
            if (campaignId == null || campaignId == Guid.Empty) { throw new ArgumentNullException(nameof(campaignId)); }
            if (page < 1) { throw new ArgumentOutOfRangeException(nameof(page)); }

            var query = _queryTranslator.Translate(column, order, searchString, page);
            return PartialView("_AdGroupTableRowsLinked", new AdGroupTableLinkedViewModel
            {
                AdGroups = _mapperAdGroup.ConvertAll(await _adGroupRepository.GetLinkedWithCampaignAsync(campaignId, query))
            });
        }

        /// <summary>
        /// Returns a partial view containing all ad groups based on some query.
        /// </summary>
        /// <param name="campaignId">Internal campaign <see cref="Guid"/></param>
        /// <param name="column"><see cref="ColumnAdGroupLinking"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <param name="searchString">Search string</param>
        /// <param name="page">Page</param>
        /// <returns><see cref="PartialViewResult"/></returns>
        [HttpGet]
        public async Task<IActionResult> GetAdGroupsAllAsync(Guid campaignId, ColumnAdGroupLinking column,
            Order order, string searchString = null, int page = 1)
        {
            if (campaignId == null || campaignId == Guid.Empty) { throw new ArgumentNullException(nameof(campaignId)); }
            if (page < 1) { throw new ArgumentOutOfRangeException(nameof(page)); }

            var query = _queryTranslator.Translate(column, order, searchString, page);
            return PartialView("_AdGroupTableRowsAll", new AdGroupTableLinkedViewModel
            {
                AdGroups = _mapperAdGroup.ConvertAll(await _adGroupRepository.GetAllAsync(query))
            });
        }

        /// <summary>
        /// Gets a partial view containing the amount of items present in our
        /// data store based on some query.
        /// </summary>
        /// <param name="campaignId">Internal campaign <see cref="Guid"/></param>
        /// <param name="column"><see cref="ColumnAdGroupLinking"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <param name="searchString">Search string</param>
        /// <param name="page">Page</param>
        /// <returns><see cref="PartialViewResult"/></returns>
        [HttpGet]
        public async Task<IActionResult> GetAdGroupsAllCountAsync(Guid campaignId, ColumnAdGroupLinking column,
            Order order, string searchString = null, int page = 1)
        {
            if (campaignId == null || campaignId == Guid.Empty) { throw new ArgumentNullException(nameof(campaignId)); }
            if (page < 1) { throw new ArgumentOutOfRangeException(nameof(page)); }

            var query = _queryTranslator.Translate(column, order, searchString, page);
            return PartialView("_AdGroupTableCountAll", new AdGroupTableAllCountViewModel
            {
                TotalCount = await _adGroupRepository.GetCountAsync(query)
            });
        }

        /// <summary>
        /// Links a given ad group to the campaign of the details view.
        /// </summary>
        /// <param name="model"><see cref="LinkingOperationViewModel"</param>
        /// <returns>No content actionresult</returns>
        [HttpPost]
        public Task<IActionResult> LinkAdGroup([FromBody] LinkingOperationViewModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unlinks a given ad group to the campaign of the details view.
        /// </summary>
        /// <param name="model"><see cref="LinkingOperationViewModel"</param>
        /// <returns>Partial view with unlinked ad groups</returns>
        [HttpPost]
        public Task<IActionResult> UnlinkAdGroup([FromBody] LinkingOperationViewModel model)
        {
            throw new NotImplementedException();
        }

    }

}
