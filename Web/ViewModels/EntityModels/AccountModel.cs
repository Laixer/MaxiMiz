using Maximiz.ViewModels.Enums;
using System;

namespace Maximiz.ViewModels.EntityModels
{

    /// <summary>
    /// Represents an account.
    /// </summary>
    public class AccountModel : EntityModel<Guid>
    {

        /// <summary>
        /// Account name in human readable form.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The publisher to which this accounts belongs.
        /// </summary>
        public Publisher Publisher { get; set; } 

    }
}
