using System.Diagnostics;
using Maximiz.ViewModels;
using Maximiz.ViewModels.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Maximiz.Controllers
{

    /// <summary>
    /// The controller for our homepage.
    /// </summary>
    public class HomeController : Controller
    {

        /// <summary>
        /// Links the home view to be loaded upon visiting the home page.
        /// </summary>
        /// <returns>The corresponding home view</returns>
        public IActionResult Index()
        {
            return View(new HomeStatisticsViewModel());
        }

        /// <summary>
        /// Returns the privacy view.
        /// </summary>
        /// <returns>The privacy view</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Returns an error view in the case of an error.
        /// </summary>
        /// <returns>The error view</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
