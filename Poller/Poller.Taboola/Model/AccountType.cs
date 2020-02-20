using System;
using System.Collections.Generic;
using System.Text;

namespace Poller.Taboola.Model
{
    
    /// <summary>
    /// Used to explicitly indicate whether an account is of type advertiser or
    /// of type publisher. The list of options might be extended in the future.
    /// </summary>
    internal enum AccountType
    {

        /// <summary>
        /// Publisher account.
        /// </summary>
        Publisher,

        /// <summary>
        /// Advertiser account.
        /// </summary>
        Advertiser

    }
}
