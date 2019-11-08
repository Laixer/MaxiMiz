using Maximiz.Controllers.Abstraction;
using Maximiz.Database.Querying;
using Maximiz.Mapper;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using Maximiz.Transactions;
using Maximiz.ViewModels.CampaignDetails;
using Maximiz.ViewModels.Columns;
using Maximiz.ViewModels.Columns.Translation;
using Maximiz.ViewModels.EntityModels;
using Maximiz.ViewModels.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for showing and editing campaign details.
    /// </summary>
    public sealed class CampaignDetailsController : Controller, ICampaignDetailsController
    {
        /// <summary>
        /// Repository containing our campaigns retrieved from the data store.
        /// </summary>
        private readonly ICampaignRepository _campaignRepository;

        /// <summary>
        /// Repository containing our ad groups retrieved from the data store.
        /// </summary>
        private readonly IAdGroupRepository _adGroupRepository;

        /// <summary>
        /// Converts our campaigns.
        /// </summary>
        private readonly IMapper<CampaignWithStats, CampaignModel> _mapperCampaign;

        /// <summary>
        /// Converts our ad groups.
        /// </summary>
        private readonly IMapper<AdGroupWithStats, AdGroupModel> _mapperAdGroup;

        /// <summary>
        /// Manages entity transactions for us.
        /// </summary>
        private readonly ITransactionHandler _transactionHandler;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="campaignRepository">The campaign repository</param>
        /// <param name="transactionHandler">The transaction handler</param>
        public CampaignDetailsController(ICampaignRepository campaignRepository,
            IAdGroupRepository adGroupRepository,
            IMapper<CampaignWithStats, CampaignModel> mapperCampaign,
            IMapper<AdGroupWithStats, AdGroupModel> mapperAdGroup,
            ITransactionHandler transactionHandler)
        {
            _campaignRepository = campaignRepository;
            _adGroupRepository = adGroupRepository;
            _mapperCampaign = mapperCampaign;
            _mapperAdGroup = mapperAdGroup;
            _transactionHandler = transactionHandler;
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
        /// <param name="model"><see cref="FormCampaignAccountViewModel"/></param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> PostFormAccount(FormCampaignAccountViewModel model)
        {
            // Do the transaction
            await Task.Delay(new Random().Next(350, 1000));
            return NoContent();
        }

        /// <summary>
        /// Saves the user submitted form variables.
        /// </summary>
        /// <param name="model"><see cref="FormCampaignMarketingViewModel"/></param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> PostFormMarketing(FormCampaignMarketingViewModel model)
        {
            // Do the transaction
            await Task.Delay(new Random().Next(350, 1000));
            return NoContent();
        }

        /// <summary>
        /// Saves the user submitted form variables.
        /// </summary>
        /// <param name="model"><see cref="FormCampaignMarketingViewModel"/></param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> PostFormPublishers(FormCampaignPublishersViewModel model)
        {
            // Do the transaction
            await Task.Delay(new Random().Next(350, 1000));
            return NoContent();
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
        /// Links a given ad group to the campaign of the details view.
        /// </summary>
        /// <param name="model"><see cref="AdGroupConnectionViewModel"</param>
        /// <returns>No content actionresult</returns>
        [HttpPost]
        public async Task<IActionResult> LinkAdGroup(Guid campaignId, Guid adGroupId)
        {
            // await _transactionHandler.HandleTransaction();
            await Task.Delay(500);
            return NoContent();
        }

        /// <summary>
        /// Unlinks a given ad group to the campaign of the details view.
        /// </summary>
        /// <param name="model"><see cref="AdGroupConnectionViewModel"</param>
        /// <returns>Partial view with unlinked ad groups</returns>
        [HttpPost]
        public async Task<IActionResult> UnlinkAdGroup(Guid campaignId, Guid adGroupId)
        {
            // await _transactionHandler.HandleTransaction()
            await Task.Delay(500);
            return NoContent();
        }

    }
}