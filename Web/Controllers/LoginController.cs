using Maximiz.ViewModels.Login;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for our login page.
    /// </summary>
    public sealed class LoginController : Controller
    {

        /// <summary>
        /// Shows the login overview.
        /// </summary>
        /// <returns><see cref="IActionResult"/></returns>
        public IActionResult Index()
            => View("Login");

        /// <summary>
        /// Attempts to login.
        /// TODO Make functional.
        /// </summary>
        /// <returns><see cref="NoContentResult"/></returns>
        public async Task<IActionResult> AttemptLogin(LoginViewModel model)
        {
            await Task.Delay(new Random().Next(500, 1500));
            return NoContent();
        }

    }
}
