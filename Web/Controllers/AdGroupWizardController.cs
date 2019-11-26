using System;
using System.Threading.Tasks;
using Maximiz.Controllers.Abstraction;
using Maximiz.Mapper;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using Maximiz.ViewModels.AdGroupWizard;
using Maximiz.ViewModels.EntityModels;
using Microsoft.AspNetCore.Mvc;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for creating ad groups.
    /// </summary>
    public class AdGroupWizardController : Controller, IAdGroupWizardController
    {

        private IAdGroupRepository _adGroupRepository;
        private IMapper<AdGroupWithStats, AdGroupModel> _mapper;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public AdGroupWizardController(IAdGroupRepository adGroupRepository,
            IMapper<AdGroupWithStats, AdGroupModel> mapper)
        {
            _adGroupRepository = adGroupRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// The start view where we select our complexity route.
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        public IActionResult ShowWizard()
            => View("Wrapper");

        /// <summary>
        /// Shows the ad group wizard as an editor, meaning all fields will alraedy
        /// be populated based on some ad group in our database.
        /// </summary>
        /// <param name="adGroupId">The internal guid of the ad group</param>
        /// <returns><see cref="ViewResult"/></returns>
        [HttpGet]
        public async Task<IActionResult> ShowWizardAsEditor(Guid adGroupId)
            => View("Wrapper", new AdGroupWizardViewModel
            {
                AdGroup = _mapper.Convert(await _adGroupRepository.Get(adGroupId))
            });

        /// <summary>
        /// TODO Remove
        /// </summary>
        /// <returns>Debug view</returns>
        [HttpGet]
        public IActionResult Debug()
        {
            return View("Debug");
        }

        /// <summary>
        /// Submits our form.
        /// </summary>
        /// <param name="model"><see cref="AdGroupFormViewModel"</param>
        /// <returns><see cref="NoContentResult"/> or <see cref="BadRequestResult"/></returns>
        [HttpPost]
        public async Task<IActionResult> SubmitForm([FromBody] AdGroupFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Simulate waiting
                await Task.Delay(new Random().Next(200, 1000));

                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns a partial view containing a single title entry.
        /// </summary>
        /// <returns><see cref="PartialViewResult"/></returns>
        [HttpGet]
        public IActionResult GetTitleEntryPartialView()
            => PartialView("_TitleEntry", new TitleEntryViewModel
            {
                Title = null, // TODO Unsafe, even though the view checks this
                IsRemovable = true
            });

    }
}
