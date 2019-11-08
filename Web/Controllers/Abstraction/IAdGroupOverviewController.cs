using Maximiz.ViewModels.AdgroupWizard;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Maximiz.Controllers.Abstraction
{

    /// <summary>
    /// Contract for our ad group overview controller.
    /// </summary>
    public interface IAdGroupOverviewController
    {

        /// <summary>
        /// Loads the overview.
        /// </summary>
        /// <returns><see cref="IActionResult"</returns>
        [HttpGet]
        IActionResult Overview();

    }
}
