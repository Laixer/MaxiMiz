using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maximiz.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for the ad group overview tables.
    /// TODO Not for this version.
    /// </summary>
    public class AdGroupOverviewController : Controller, IAdGroupOverviewController
    {

        /// <summary>
        /// Loads our overview.
        /// </summary>
        /// <returns></returns>
        public IActionResult Overview()
        {
            return View("Overview");
        }
    }
}