using Microsoft.AspNetCore.Mvc;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for creating new campaign groups.
    /// </summary>
    public class CampaignGroupController : Controller
    {

        /// <summary>
        /// Index view.
        /// TODO Update doc
        /// </summary>
        /// <returns>The default index view</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}
