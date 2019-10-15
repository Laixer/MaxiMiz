using Maximiz.InputModels;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{
    /// <summary>
    /// Controller for handling requests related to Ad Groups.
    /// </summary>
    public class AdGroupController : Controller
    {
        private readonly IAdGroupRepository _adGroupRepository;

        public AdGroupController(IAdGroupRepository adGroupRepository)
        {
            _adGroupRepository = adGroupRepository;
        }

        // GET: /AdGroup/Create
        /// <summary>
        /// The Create page for creating a new <see cref="AdGroup"></see>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            // Create an empty model for the first time loading the create form.
            var emptyModel = new AdGroupInputModel
            {
                AdItems = new List<AdItemInputModel>()
            };

            // Initially add 5 empty ad items, to be viewed within the create form.
            for (int i = 0; i < 5; i++)
            {
                emptyModel.AdItems.Add(new AdItemInputModel());
            }

            return View(emptyModel);
        }
        //TODO: Make this work
        /// <summary>
        /// Uploading a picture from the local device.
        /// </summary>
        /// <param name="uploadFile">The picture that needs to be uploaded</param>
        /// <returns>A view with the picture that you uploaded</returns>
        [HttpPost]
        public Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null)
            {
               
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adding a inputmodel for the inputgroup
        /// </summary>
        /// <param name="model">Model that needs the inputmodel</param>
        /// <returns>The same view with a extra inputmodel</returns>
        public IActionResult AddAdv(AdGroupInputModel model)
        {
            model.AdItems.Add(new AdItemInputModel());
            return View(model);
        }

        /// <summary>
        /// Deleting a inputmodel for the inputgroup
        /// </summary>
        /// <param name="model">Model that doesnt need the inputmodel</param>
        /// <returns>the same view with one inputmodel less</returns>
        [HttpGet]
        public IActionResult DeleteAdv(AdGroupInputModel model)
        {
            //model.AdItems.RemoveAt(item);
            return View(model);
        }
        // POST: /AdGroup/Create
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
            // Remove empty ads before sending the model
            model.AdItems.RemoveAll(x => string.IsNullOrEmpty(x.Title) && string.IsNullOrEmpty(x.Content));

            await _adGroupRepository.CreateGroup(model);

            // TODO: Determine where to redirect after a succesful Ad Group creation. (AdGroup overview?)
            return RedirectToAction("Index", "Home");
        }
    }
}