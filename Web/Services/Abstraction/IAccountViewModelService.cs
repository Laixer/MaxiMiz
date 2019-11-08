using Maximiz.ViewModels.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.Services.Abstraction
{

    /// <summary>
    /// Contract for an account viewmodel service.
    /// </summary>
    public interface IAccountViewModelService
    {

        /// <summary>
        /// Gets all currently available advertiser accounts.
        /// </summary>
        /// <returns>List of <see cref="AccountModel"/>s</returns>
        Task<IEnumerable<AccountModel>> GetAdvertiserAccounts();

    }
}
