using Maximiz.Storage.Abstraction;
using Maximiz.ViewModels;
using Maximiz.ViewModels.ImagePicker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for our image picker.
    /// </summary>
    public sealed class ImagePickerController : Controller
    {

        /// <summary>
        /// Handles our storage.
        /// </summary>
        private IStorageManager _storageManager;

        /// <summary>
        /// Construcor for dependency injection.
        /// </summary>
        public ImagePickerController(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }

        /// <summary>
        /// Shows the image picker.
        /// </summary>
        /// <returns><see cref="ViewResult"/></returns>
        public IActionResult Index()
            => PartialView("_ImagePicker");

        /// <summary>
        /// Uploads an image to our storage.
        /// </summary>
        /// <param name="file"><see cref="IFormFile"/></param>
        /// <returns><see cref="NoContentResult"/> or <see cref="BadRequestResult"/></returns>
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null) { return BadRequest(); }
            if (!file.ContentType.Contains("image"))
            {
                return BadRequest();
            } // TODO This does NOT seem bulletproof

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
        /// Gets a partial view displaying a list of images the user uploaded.
        /// </summary>
        /// <param name="query">Search query string</param>
        /// <returns><see cref="PartialViewResult"/></returns>
        [HttpGet]
        public async Task<IActionResult> GetUploadedImages()
            => PartialView("_ImageList", new ImageListViewModel
            {
                ImageUris = await _storageManager.GetUploadedImages("")
            });

        /// <summary>
        /// Gets a partial view displaying a list of selected image uris.
        /// TODO This can be cleaned up
        /// </summary>
        /// <remarks>
        /// This is a post because we might have to send a long list of image
        /// uris to the server. Some browsers might disable an url that is too
        /// long, hence we are not placing the uris in the url itself.
        /// </remarks>
        /// <param name="imageUris">List of image <see cref="Uri"/>s</param>
        /// <returns><see cref="PartialViewResult"/></returns>
        [HttpPost]
        public IActionResult GetSelectedImagesThumbnails([FromBody] ImageUrisViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ImageSelectedThumbnails", new ImageListViewModel
                {
                    ImageUris = new List<Uri>()
                });
            }

            // It's possible to receive a list with an empty or null string as its only item
            // TODO Maybe fix this in model validation
            if (model.ImageUris != null && (
            model.ImageUris.ToList()[0] == null ||
            string.IsNullOrEmpty(model.ImageUris.ToList()[0].ToString()))) // TODO This was not tostring but absoluteuri
            {
                return PartialView("_ImageSelectedThumbnails", new ImageListViewModel
                {
                    ImageUris = new List<Uri>()
                });
            };

            return PartialView("_ImageSelectedThumbnails", new ImageListViewModel
            {
                ImageUris = model.ImageUris
            });
        }
    }
}
