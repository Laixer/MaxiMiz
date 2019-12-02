using Maximiz.ViewModels.EntityModels;
using System.Collections.Generic;

namespace Maximiz.ViewModels.Settings
{

    /// <summary>
    /// Viewmodel for displaying all linked accounts.
    /// </summary>
    public sealed class LinkedAccountsTableViewModel
    {

        /// <summary>
        /// List of linked accounts we wish to display.
        /// </summary>
        public IEnumerable<AccountModel> LinkedAccounts { get; set; }

    }
}
