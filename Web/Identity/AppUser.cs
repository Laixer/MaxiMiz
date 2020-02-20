using Microsoft.AspNetCore.Identity;
using System;

namespace Maximiz.Identity
{

    /// <summary>
    /// Application user.
    /// </summary>
    public sealed class AppUser : IdentityUser<Guid>
    {

        /// <summary>
        /// User first name.
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// User last name.
        /// </summary>
        public string LastName { get; set; }

    }

}
