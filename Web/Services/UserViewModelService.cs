using Maximiz.Identity;
using Maximiz.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Maximiz.Services
{

    /// <summary>
    /// Allows our views to extract user data for display.
    /// </summary>
    public sealed class UserViewModelService : IUserViewModelService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public UserViewModelService(UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets and formats the user email.
        /// </summary>
        /// <returns>User email as string</returns>
        public async Task<string> GetEmail()
        {
            var userClaim = _httpContextAccessor.HttpContext.User;
            var user = await _userManager.GetUserAsync(userClaim);

            if (user != null) { return user.Email; }
            else { return "this_should@notbe.visible"; } // TODO Leave or remove this?
        }
    }
}
