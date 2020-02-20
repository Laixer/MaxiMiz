
using System.ComponentModel.DataAnnotations;

namespace Maximiz.ViewModels.Login
{

    /// <summary>
    /// Viewmodel for our login page.
    /// </summary>
    public sealed class LoginViewModel
    {

        /// <summary>
        /// User email.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [Required]
        public string Password { get; set; }

    }
}
