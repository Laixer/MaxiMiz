using Maximiz.Identity;
using Maximiz.ViewModels.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for our login page.
    /// </summary>
    public sealed class LoginController : Controller
    {

        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private ILogger logger;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public LoginController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            logger = loggerFactory.CreateLogger(nameof(LoginController));
        }

        /// <summary>
        /// Shows the login overview.
        /// </summary>
        /// <returns><see cref="IActionResult"/></returns>
        [AllowAnonymous]
        public IActionResult Index()
            => View("Login");

        /// <summary>
        /// Attempts to login using Identity Framework.
        /// TODO Give user failed login feedback.
        /// </summary>
        /// <returns><see cref="NoContentResult"/> or <see cref="BadRequestResult"/></returns>
        [AllowAnonymous]
        public async Task<IActionResult> AttemptLogin([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            try
            {
                var result = await _signInManager.PasswordSignInAsync
                    (model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    logger.LogTrace("Logged in user");
                    return NoContent();
                }
                else
                {
                    logger.LogTrace("Couldn't log in user, returning view");
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Could not login user, message: {e.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Performs user logout.
        /// </summary>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                logger.LogTrace("Logged out user");
            }
            catch (Exception e)
            {
                logger.LogError($"Error while logging out user, message: {e.Message}");
            }
            return NoContent();
        }

        /// <summary>
        /// Attempts to register a user.
        /// TODO Make callable
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        [AllowAnonymous]
        private async Task Register()
        {
            logger.LogInformation("Adding our debug user to the database.");
            try
            {
                var user = await _userManager.FindByNameAsync("TestUser");
                if (user == null)
                {
                    user = new AppUser();
                    user.UserName = "TestUser";
                    user.Email = "testuser@laixer.com";

                    var result = await _userManager.CreateAsync(user, "Welkom01!");
                    logger.LogTrace("Created new user in database.");
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Error while registering user, message: {e.Message}");
            }
        }
    }

}
