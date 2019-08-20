using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Interfaces;
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
        public IActionResult New()
        {
            return View();
        }

        [HttpGet, Route("Campaign/New/Basic")]
        public IActionResult NewBasic()
        {
            return View("~/Views/Campaign/Create/Basic.cshtml");
        }

        [HttpGet, Route("Campaign/New/Advanced")]
        public IActionResult NewAdvanced()
        {
            return View("~/Views/Campaign/Create/Advanced.cshtml");
        }

        [HttpGet]
        public IEnumerable<Campaign> Search(string query)
        {
            return campaignRepo.Search(query);
        }

        [HttpPut]
        public IActionResult Edit(Campaign campaign)
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult Duplicate(Campaign campaign)
        {
            return null;
        }

        [HttpDelete]
        public IActionResult Delete(Campaign campaign)
        {
            return null;
        }

        public IActionResult TestCreate()
        {
             campaignRepo.Create(
                new Campaign
                {
                    BrandingText = "Test",
                    InitialCpc = 0.01M,
                    Budget = 1000M,
                    DailyBudget = 100M,
                    Utm = "ABC"
                }
            );
            return Ok("Campaign was created.");
        }

    }
}