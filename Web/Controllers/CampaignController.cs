using Maximiz.Database;
using Maximiz.InputModels;
using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using Maximiz.Models;
using Maximiz.Repositories.Abstraction;
using Maximiz.Transactions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for requests related to campaigns.
    /// </summary>
    public class CampaignController : Controller
    {

        /// <summary>
        /// Repository containing our campaigns retrieved from the database.
        /// </summary>
        private readonly ICampaignRepository _campaignRepository;

        /// <summary>
        /// Manages entity transactions for us.
        /// </summary>
        private readonly ITransactionHandler _transactionHandler;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="campaignRepository">The campaign repository</param>
        /// <param name="transactionHandler">The transaction handler</param>
        public CampaignController(ICampaignRepository campaignRepository, ITransactionHandler transactionHandler)
        {
            _campaignRepository = campaignRepository;
            _transactionHandler = transactionHandler;
        }

        /// <summary>
        /// Index page for the campaign page, redirecting us to the overview.
        /// GET: /Campaign/
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Overview");
        }

        /// <summary>
        /// An overview of all campaigns.
        /// GET: /Campaign/Overview
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Overview()
        {
            return View(new OverviewCampaignModel
            {
                Campaigns = await _campaignRepository.GetAll()
            });
        }

        /// <summary>
        /// Http Get method for a sorted overview of the campaigns. This does
        /// query our own database.
        /// 
        /// TODO Do we need this overload?
        /// TODO Should we always re-query the database?
        /// </summary>
        /// <param name="column">The column to order by</param>
        /// <param name="order">The order</param>
        /// <returns>The view</returns>
        [HttpGet]
        public async Task<IActionResult> OverviewSorted(ColumnCampaign column, Order order)
        {
            return View("Overview", new OverviewCampaignModel
            {
                Campaigns = await _campaignRepository.GetAll(column, order)
            });
        }

        /// <summary>
        /// Http post method to search our database based on a user provided
        /// search query.
        /// </summary>
        /// <param name="query">The search query</param>
        /// <returns>View with retrieved campaigns</returns>
        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            // TODO Do nothing if we have no query
            if (string.IsNullOrEmpty(query)) { return RedirectToAction("Overview"); }

            // Create and return view with model
            var model = new OverviewCampaignModel
            {
                Campaigns = await _campaignRepository.Search(query)
            };
            return View("Overview", model);

            //return View("OverView", new OverviewCampaignModel { Campaigns = await _campaignRepository.GetAll() });
        }

        [HttpGet]
        public async Task<IActionResult> MyAction(int i)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> OnPressActive()
        {
            return RedirectToAction("Overview");
        }



        /// <summary>
        /// Displaying the details for a single <see cref="Campaign"/>.
        /// GET: /Campaign/Details/{Id}
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var c = await _campaignRepository.Get(id);
            return View(c);
        }

        /// <summary>
        /// The create page for a new <see cref="Campaign"/>.
        /// GET: /Campaign/Create
        /// </summary>
        /// <returns>Action result</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create a new <see cref="CampaignGroup"/> entity and generate its 
        /// corresponding <see cref="Campaign"/> entities.
        /// POST: /Campaign/Create
        /// </summary>
        /// <param name="input">The input model for a new campaign</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CampaignGroupInputModel input)
        {
            // TODO: Validate and create model, handle possible error
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            //CampaignGroup group = input.ToEntityModel();

            throw new NotImplementedException();
            // await _campaignRepository.CreateGroup(group);

            return RedirectToAction("Overview");
        }

        // TODO Remove, temporary page for quickly creating a model with a prefilled  form
        public IActionResult CreateTest(string name)
        {
            return View("Create", new CampaignGroupInputModel()
            {
                Name = name ?? "Test",
                Budget = 9001,
                DailyBudget = 10,
                Devices = new Device[] { Device.Desktop, Device.Laptop, Device.Mobile, Device.Tablet, Device.Wearable },
                OperatingSystems = new OS[] { OS.Android, OS.Chromeos, OS.iOS, },
                //ConnectionsType = new ConnectionType[] { ConnectionType.Wifi },
                Utm = "utm=test",
                BrandingText = "Test",
            });
        }



        [HttpPut]
        public IActionResult Edit(Campaign campaign)
        {
            // TODO
            return NotFound();
            _campaignRepository.Update(campaign);
        }

        [HttpPut]
        public IActionResult Duplicate(Campaign campaign)
        {
            // TODO
            return NotFound();
            _campaignRepository.Create(campaign);
        }

        [HttpDelete]
        public IActionResult Delete(Campaign campaign)
        {
            // TODO
            return NotFound();
            _campaignRepository.Delete(campaign);
        }
    }
}