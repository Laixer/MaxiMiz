using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maximiz.InputModels;
using Maximiz.Model.Entity;
using Maximiz.Repositories;
using Maximiz.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maximiz.Controllers
{
    public class AdGroupController : Controller
    {
        private IAdGroupRepository _adGroupRepository;

        public AdGroupController(IAdGroupRepository adGroupRepository)
        {
            _adGroupRepository = adGroupRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdGroupInputModel model)
        {
            await _adGroupRepository.CreateGroup(model);

            return View();
        }
    }
}