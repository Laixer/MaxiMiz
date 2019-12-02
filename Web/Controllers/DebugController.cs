using Maximiz.Storage;
using Maximiz.Storage.Abstraction;
using Maximiz.ViewModels.CampaignOverview;
using Maximiz.ViewModels.Debug;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller used for debug purposes.
    /// </summary>
    public class DebugController : Controller
    {

        private readonly IStorageManager _storageManager;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public DebugController(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }

        /// <summary>
        /// Uploads a file.
        /// </summary>
        /// <param name="file"><see cref="IFormFile"/></param>
        /// <returns><see cref="NoContentResult"/> or <see cref="BadRequestResult"/></returns>
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null) { return BadRequest(); }
            if (!file.ContentType.Contains("image")) { return BadRequest(); } // TODO This does NOT seem bulletproof

            if (await _storageManager.UploadFile(file))
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets all images from the storage folder.
        /// </summary>
        /// <returns><see cref="View"/></returns>
        [HttpGet]
        public async Task<IActionResult> GetImages()
            => View("ImageList", new DebugImageListViewModel
            {
                ImageUris = await _storageManager.GetUploadedImages()
            });



        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DoSimpleGet()
        {
            return PartialView("Default");
        }

        [HttpGet]
        public IActionResult DoGet(int number, string name)
        {
            return PartialView("Default");
        }

        [HttpPost]
        public IActionResult DoPost([FromQuery]int number, [FromBody] AdGroupOverviewCountViewModel model)
        {
            if (model.AdGroupCount != 0 && model.TableName != null)
            {

            }
            return PartialView("Default");
        }

    }
}