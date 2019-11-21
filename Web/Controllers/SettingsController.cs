using Maximiz.ViewModels.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for our settings.
    /// </summary>
    public sealed class SettingsController : Controller
    {

        /// <summary>
        /// Shows our settings page containing all existing tabs. This method
        /// also selects which tab we wish to show the user first.
        /// </summary>
        /// <remarks>The default settings tab is <see cref="SettingsTab.Account"</remarks>
        /// <param name="settingsTab">The tab we wish to show the user</param>
        /// <returns><see cref="ViewResult"/></returns>
        public IActionResult ShowSettings(SettingsTab settingsTab = SettingsTab.Account)
            => View("Wrapper", new SettingsViewModel { SettingsTab = settingsTab });

    }
}
