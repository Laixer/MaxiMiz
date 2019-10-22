using Maximiz.Model.Entity;
using Maximiz.Model.Enums;

namespace Maximiz.AccountManagement
{

    /// <summary>
    /// Interface for keeping track of which accounts we currently want our 
    /// created entities to belong to. The implementation should simply store 
    /// the selected accounts for each publisher.
    /// </summary>
    internal interface IAccountManager
    {

        /// <summary>
        /// Gets the account for a given <see cref="Publisher"/>.
        /// </summary>
        /// <param name="publisher"><see cref="Publisher"/></param>
        /// <returns>The corresponding <see cref="Account"/></returns>
        Account GetSelectedPublisherAccount(Publisher publisher);

    }
}
