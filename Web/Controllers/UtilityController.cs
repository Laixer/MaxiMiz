using Microsoft.AspNetCore.Mvc;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Contains some utility functions for us.
    /// </summary>
    public class UtilityController : Controller
    {

        /// <summary>
        /// Gets the loading icon for us.
        /// </summary>
        /// <returns>Partial view containing the loading icon</returns>
        public IActionResult LoadingIcon()
        {
            return PartialView("_LoadingIcon");
        }
    }
}