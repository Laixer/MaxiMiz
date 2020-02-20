using System.Threading.Tasks;

namespace Maximiz.Services.Abstraction
{

    /// <summary>
    /// Contract for displaying information about the user.
    /// </summary>
    public interface IUserViewModelService
    {

        /// <summary>
        /// Gets the email of the user that's currently logged in.
        /// TODO Is this a security flaw?
        /// </summary>
        /// <returns>Email address</returns>
        Task<string> GetEmail();

    }

}
