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

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Overview");
        }

        /// <summary>
        /// An overview of all campaigns
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Overview()
        {
            var allCampaigns = await _campaignRepo.GetAll();
            return View(allCampaigns);
        }

        /// <summary>
        /// Displaying the details for a single <see cref="Campaign"></see>
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var c = await _campaignRepo.Get(id);
            return View(c);
        }

        /// <summary>
        /// The create page for a new <see cref="Campaign"></see>
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">The input model for a new campaign</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(CampaignGroupInputModel model)
        {
            // TODO: Validate and create model, handle possible error
            if (!ModelState.IsValid) {
                return View(model);
            }

            //var campaign = new Campaign
            //{
            //    Utm = model.Url,
            //    InitialCpc = model.InitialCpc,
            //    BrandingText = model.BrandingText,
            //};
            //
            //_campaignRepo.Create(campaign);

            return View(model);

            return RedirectToAction("Overview");
        }

        /// <summary>
        /// Query the database to search for campaigns
        /// </summary>
        /// <param name="query">The search term</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Campaign>> Search(string query) => await _campaignRepo.Search(query);

        [HttpPut]
        public IActionResult Edit(Campaign campaign)
        {
            _campaignRepo.Update(campaign);

            return StatusCode(500, "Error");
        }

        [HttpPut]
        public IActionResult Duplicate(Campaign campaign)
        {
            // TODO
            return null;
        }

        [HttpDelete]
        public IActionResult Delete(Campaign campaign)
        {
            // TODO
            return null;
        }

    }
}