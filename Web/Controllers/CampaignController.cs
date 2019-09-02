using Maximiz.InputModels;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{
    // TODO make methods asynchronous (busy)
    public class CampaignController : Controller
    {
        private readonly ICampaignRepository _campaignRepo;

        public CampaignController(ICampaignRepository campaignRepo)
        {
            _campaignRepo = campaignRepo;
        }

        // GET: /Campaign/
        /// <summary>
        /// Index page for campaigns
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Overview");
        }

        // GET: /Campaign/Overview
        /// <summary>
        /// An overview of all campaigns
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Overview()
        {
            var allCampaigns = await _campaignRepo.GetAll();

            return View(allCampaigns);
        }

        // GET: /Campaign/Details/{Id}
        /// <summary>
        /// Displaying the details for a single <see cref="Campaign"></see>
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var c = await _campaignRepo.Get(id);
            return View(c);
        }

        // GET: /Campaign/Create
        /// <summary>
        /// The create page for a new <see cref="Campaign"></see>
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Campaign/Create
        /// <summary>
        /// Create a new <see cref="CampaignGroup"></see> entity and generate its corresponding <see cref="Campaign"></see> entities.
        /// </summary>
        /// <param name="input">The input model for a new campaign</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CampaignGroupInputModel input)
        {
            // TODO: Validate and create model, handle possible error
            if (!ModelState.IsValid) {
                return View(input);
            }

            CampaignGroup group = input.ToEntityModel();

            await _campaignRepo.CreateGroup(group);

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
                Devices = new Model.Enums.Device[] { Model.Enums.Device.Desktop, Model.Enums.Device.Laptop, Model.Enums.Device.Mobile, Model.Enums.Device.Tablet, Model.Enums.Device.Wearable },
                OperatingSystems = new Model.Enums.OS[] { Model.Enums.OS.Android, Model.Enums.OS.Chromeos, Model.Enums.OS.Ios, },
                Connections = new Model.Enums.Connection[] { Model.Enums.Connection.Wifi },
                Utm = "utm=test",
                BrandingText = "Test",
            });
        }

        // GET: /Campaign/Search
        /// <summary>
        /// Query the database to search for campaigns.
        /// </summary>
        /// <param name="query">The search term to query the database for.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Campaign>> Search(string query) => await _campaignRepo.Search(query);

        // PUT: /Campaign/Edit
        /// <summary>
        /// Update an existing <see cref="Campaign"></see> entity.
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Edit(Campaign campaign)
        {
            // TODO
            return NotFound();
            _campaignRepo.Update(campaign);
        }

        [HttpPut]
        public IActionResult Duplicate(Campaign campaign)
        {
            // TODO
            return NotFound();
            _campaignRepo.Create(campaign);
        }

        [HttpDelete]
        public IActionResult Delete(Campaign campaign)
        {
            // TODO
            return NotFound();
            _campaignRepo.Delete(campaign);
        }

    }
}