using Maximiz.Services.Abstraction;
using Maximiz.ViewModels.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Services
{

    /// <summary>
    /// Handles our account display for use in view models.
    /// </summary>
    public sealed class AccountViewModelService : IAccountViewModelService
    {

        /// <summary>
        /// Gets all currently available advertiser accounts.
        /// </summary>
        /// <returns>List of <see cref="AccountModel"/>s</returns>
        public async Task<IEnumerable<AccountModel>> GetAdvertiserAccounts()
        {
            // TODO Implement
            return new List<AccountModel>
            {
                new AccountModel() {Name = "My first account", Id = Guid.NewGuid() },
                new AccountModel() {Name = "My second account", Id = Guid.NewGuid() },
                new AccountModel() {Name = "My third account", Id = Guid.NewGuid() },
            };
        }

    }
}
