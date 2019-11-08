using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return View("Index");
        }

    }
}
