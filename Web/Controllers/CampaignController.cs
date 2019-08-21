using System;
using System.Collections.Generic;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Interfaces;
using Maximiz.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Maximiz.Controllers
{
    public class CampaignController : Controller
    {
        private readonly ICampaignRepository campaignRepo;

        public CampaignController(ICampaignRepository campaignRepo)
        {
            this.campaignRepo = campaignRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Overview");
        }

        [HttpGet]
        public IActionResult Overview()
        {
            return View(campaignRepo.GetAll());
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            return View(campaignRepo.Get(id));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateCampaignViewModel model)
        {
            // TODO: Validate and create model, handle possible error
            if (ModelState.IsValid) {

                Campaign campaign = new Campaign()
                {
                    Utm = model.URL,
                    InitialCpc = model.CPC,
                    BrandingText = model.Branding,
                };

                campaignRepo.Create(campaign);
            }

            return RedirectToAction("Overview");
        }

        [HttpGet]
        public IEnumerable<Campaign> Search(string query)
        {
            return campaignRepo.Search(query);
        }

        [HttpPut]
        public IActionResult Edit(Campaign campaign)
        {
            // TODO: Check validity
            campaignRepo.Update(campaign);

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