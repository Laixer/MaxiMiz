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
        private readonly IAdGroupRepository _adGroupRepository;

        public AdGroupController(IAdGroupRepository adGroupRepository)
        {
            _adGroupRepository = adGroupRepository;
        }

        /// <summary>
        /// The Create page for creating a new <see cref="AdGroup"></see>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// <para>
        /// Creates and inserts an <see cref="AdGroup"></see> entity into the database.
        /// </para>
        /// <para>
        /// Also generates <see cref="AdItem"></see> entities for every
        /// item contained in <see cref="AdGroupInputModel.AdItems"></see>.
        /// </para>
        /// </summary>
        /// <param name="model">The input model from the form.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(AdGroupInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _adGroupRepository.CreateGroup(model);

            // TODO: Determine where to redirect after a succesful Ad Group creation.
            return RedirectToAction("Index", "Home");
        }
    }
}