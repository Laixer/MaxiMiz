using Maximiz.Controllers.Abstraction;
using Maximiz.Core.StateMachine.Abstraction;
using Maximiz.Database.Querying;
using Maximiz.Mapper;
using Maximiz.Model.Entity;
using Maximiz.Operations;
using Maximiz.Repositories.Abstraction;
using Maximiz.ViewModels.CampaignDetails;
using Maximiz.ViewModels.Columns;
using Maximiz.ViewModels.Columns.Translation;
using Maximiz.ViewModels.EntityModels;
using Maximiz.ViewModels.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for showing and editing campaign details.
    /// </summary>
    public sealed class CampaignDetailsController : Controller, ICampaignDetailsController
    {
        
        private readonly ICampaignRepository _campaignRepository;
        private readonly IAdGroupRepository _adGroupRepository;
        private readonly IMapper<CampaignWithStats, CampaignModel> _mapperCampaign;
        private readonly IMapper<AdGroupWithStats, AdGroupModel> _mapperAdGroup;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public CampaignDetailsController(ICampaignRepository campaignRepository,
            IAdGroupRepository adGroupRepository,
            IMapper<CampaignWithStats, CampaignModel> mapperCampaign,
            IMapper<AdGroupWithStats, AdGroupModel> mapperAdGroup,
            ILoggerFactory loggerFactory)
        {
            _campaignRepository = campaignRepository ?? throw new ArgumentNullException(nameof(campaignRepository));
            _adGroupRepository = adGroupRepository ?? throw new ArgumentNullException(nameof(adGroupRepository));
            _mapperCampaign = mapperCampaign ?? throw new ArgumentNullException(nameof(mapperCampaign));
            _mapperAdGroup = mapperAdGroup ?? throw new ArgumentNullException(nameof(mapperAdGroup)) ;

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
            return PartialView("_Details", new CampaignDetailsViewModel
            {
                Campaign = _mapperCampaign.Convert(await _campaignRepository.Get(id)),
                Account = new AccountModel() { Name = "This is my account oh yes oh yes", Publisher = Publisher.Taboola } // TODO Account repository
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

            // Await
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
        /// Returns the partial view containing all currently linked ad groups.
        /// </summary>
        /// <param name="campaignId">The id of the corresponding campaign</param>
        /// <returns>Partial view</returns>
        [HttpGet]
        public IActionResult GetAdGroupsLinkedViewComponent(Guid campaignId, string query, ColumnAdGroupLinking column, Order order)
        {
            // Translate from viewmodel to model
            var columnTranslated = ColumnTranslator.Translate(column);
            var orderTranslated = OrderTranslator.Translate(order);

            return ViewComponent("AdGroupTableLinked", new
            {
                campaignId,
                query = new QueryAdGroupWithStats(query, columnTranslated, orderTranslated)
            });
        }

        /// <summary>
        /// Returns the partial view containing all currently linked ad groups.
        /// </summary>
        /// <param name="campaignId">The id of the corresponding campaign</param>
        /// <returns>Partial view</returns>
        [HttpGet]
        public IActionResult GetAdGroupsAllViewComponent(Guid campaignId, string query, ColumnAdGroupLinking column, Order order)
        {
            // Translate from viewmodel to model
            var columnTranslated = ColumnTranslator.Translate(column);
            var orderTranslated = OrderTranslator.Translate(order);

            return ViewComponent("AdGroupTableAll", new
            {
                campaignId,
                query = new QueryAdGroupWithStats(query, columnTranslated, orderTranslated)
            });
        }

        /// <summary>
        /// Gets the count view component.
        /// </summary>
        /// <param name="campaignId">Corresponding campaign id</param>
        /// <param name="query">Query string</param>
        /// <param name="column"><see cref="ColumnAdGroupLinking"/></param>
        /// <param name="order"><see cref="Order"/></param>
        /// <returns><see cref="ViewComponent"/></returns>
        [HttpGet]
        public IActionResult GetAdGroupsAllCountViewComponent(Guid campaignId, string query, ColumnAdGroupLinking column, Order order)
        {
            // Translate from viewmodel to model
            var columnTranslated = ColumnTranslator.Translate(column);
            var orderTranslated = OrderTranslator.Translate(order);

            return ViewComponent("AdGroupTableAllCount", new
            {
                campaignId,
                query = new QueryAdGroupWithStats(query, columnTranslated, orderTranslated)
            });
        }

        /// <summary>
        /// Links a given ad group to the campaign of the details view.
        /// </summary>
        /// <param name="model"><see cref="LinkingOperationViewModel"</param>
        /// <returns>No content actionresult</returns>
        [HttpPost]
        public async Task<IActionResult> LinkAdGroup([FromBody] LinkingOperationViewModel model)
        {
            // await _transactionHandler.HandleTransaction();
            await Task.Delay(500);
            return NoContent();
        }

        /// <summary>
        /// Unlinks a given ad group to the campaign of the details view.
        /// </summary>
        /// <param name="model"><see cref="LinkingOperationViewModel"</param>
        /// <returns>Partial view with unlinked ad groups</returns>
        [HttpPost]
        public async Task<IActionResult> UnlinkAdGroup([FromBody] LinkingOperationViewModel model)
        {
            // await _transactionHandler.HandleTransaction()
            await Task.Delay(500);
            return NoContent();
        }

    }

}
