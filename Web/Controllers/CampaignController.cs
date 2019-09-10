using Maximiz.InputModels;
using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using Maximiz.Models;
using Maximiz.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{
    /// <summary>
    /// Controller for requests related to Campaigns.
    /// </summary>
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
        /// An overview of all campaigns.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Overview()
        {
            return View(new OverviewCampaignModel { Campaigns = await _campaignRepo.GetAll() });
        }

        /// <summary>
        /// A overview which can be sorted
        /// It can be used after the page is loaded
        /// </summary>
        /// <param name="order"> Enum Order which order you want the type to be</param>
        /// <param name="type">Top row of the table with all the types in it</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> OverviewSorted(Order order, CampaignModel type)
        {
            return View("Overview", new OverviewCampaignModel { Campaigns = await _campaignRepo.GetAll(type, order) });
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
            if (!ModelState.IsValid)
            {
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
                Devices = new Device[] { Device.Desktop, Device.Laptop, Device.Mobile, Device.Tablet, Device.Wearable },
                OperatingSystems = new OS[] { OS.Android, OS.Chromeos, OS.Ios, },
                Connections = new Connection[] { Connection.Wifi },
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
        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                var model = new OverviewCampaignModel
                {
                    Campaigns = await _campaignRepo.Search(query)
                };
                return View("OverView", model);
            }

            return View("OverView", new OverviewCampaignModel { Campaigns = await _campaignRepo.GetAll()});
        }

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