using Maximiz.Identity;
using Maximiz.Mapper;
using Maximiz.Model.Entity;
using Maximiz.Repositories.Abstraction;
using Maximiz.ViewModels.EntityModels;
using Maximiz.ViewModels.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{

    /// <summary>
    /// Controller for our settings.
    /// </summary>
    public sealed class SettingsController : Controller
    {

        private readonly IAccountRepository _accountRepository;
        private readonly IMapper<Account, AccountModel> _mapperAccount;
        private readonly UserManager<AppUser> _userManager;

        /// <summary>
        /// Constructor for depenedency injection.
        /// </summary>
        public SettingsController(IAccountRepository accountRepository,
            IMapper<Account, AccountModel> mapperAccount, UserManager<AppUser> userManager)
        {
            _accountRepository = accountRepository;
            _mapperAccount = mapperAccount;
            _userManager = userManager;
        }

        /// <summary>
        /// Shows our settings page containing all existing tabs. This method
        /// also selects which tab we wish to show the user first.
        /// </summary>
        /// <remarks>The default settings tab is <see cref="SettingsTab.Account"</remarks>
        /// <param name="settingsTab">The tab we wish to show the user</param>
        /// <returns><see cref="ViewResult"/></returns>
        public IActionResult ShowSettings(SettingsTab settingsTab = SettingsTab.Account)
            => View("Wrapper", new SettingsViewModel { SettingsTab = settingsTab });

        /// <summary>
        /// Returns the <see cref="PartialViewResult"/> where all linked accounts
        /// are displayed in a table.
        /// </summary>
        /// <returns><see cref="PartialViewResult"/></returns>
        public async Task<IActionResult> GetListedAccountsPartialView()
            => PartialView("_LinkedAccountsTableBody", new LinkedAccountsTableViewModel
            {
                LinkedAccounts = _mapperAccount.ConvertAll(await _accountRepository.GetAll())
            });

        /// <summary>
        /// Gets the details of our currently logged in account.
        /// </summary>
        /// <returns><see cref="PartialViewResult"/></returns>
        public async Task<IActionResult> GetAccountDetailsPartialView()
        {
            var user = await _userManager.GetUserAsync(User);
            return PartialView("_PageAccountPopulated", new SettingsAccountViewModel
            {
                FirstName = user.GivenName,
                LastName = user.LastName,
                Email = user.Email,
            });
        }
    }
}
