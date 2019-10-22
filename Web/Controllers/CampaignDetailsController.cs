using System;
using System.Threading.Tasks;
using Maximiz.Mapper;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using Maximiz.Transactions;
using Maximiz.ViewModels;
using Maximiz.ViewModels.EntityModels;
using Microsoft.AspNetCore.Mvc;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for showing and editing campaign details.
    /// </summary>
    internal sealed class CampaignDetailsController : Controller
    {
        /// <summary>
        /// Repository containing our campaigns retrieved from the database.
        /// </summary>
        private readonly ICampaignRepository _campaignRepository;

        /// <summary>
        /// Converts our campaigns.
        /// </summary>
        private readonly IMapper<Campaign, CampaignModel> _mapperCampaign;

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
            IMapper<Campaign, CampaignModel> mapperCampaign, ITransactionHandler transactionHandler)
        {
            _campaignRepository = campaignRepository;
            _mapperCampaign = mapperCampaign;
            _transactionHandler = transactionHandler;
        }

        public ActionResult Index()
        {
            return View();
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
            var campaign = await _campaignRepository.Get(id);
            var campaignConverted = _mapperCampaign.Convert(campaign);
            return View("Index", new CampaignDetailsViewModel
            {
                Campaign = campaignConverted
            });
        }
    }
}