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
        private readonly ICampaignRepository _campaignRepo;

        public CampaignController(ICampaignRepository campaignRepo)
        {
            _campaignRepo = campaignRepo;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Overview");
        }

        public IActionResult Overview()
        {
            return View(_campaignRepo.GetAllCampaigns().Result);
        }

        public IActionResult Details(Guid id)
        {
            return View(_campaignRepo.GetCampaign(id).Result);
        }

        public IActionResult New()
        {
            return View();
        }

        public IActionResult TestCreate()
        {
            _campaignRepo.CreateCampaignTest(
                new Campaign
                {
                    BrandingText = "Test",
                    InitialCpc = 0.01M,
                    Budget = 1000M,
                    DailyBudget = 100M,
                    Utm = "ABC"
                }
                );
            return Ok("OK");
        }

    }
}