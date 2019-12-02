using Maximiz.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Repositories.Abstraction
{

    /// <summary>
    /// Contract for an <see cref="Account"/> repository.
    /// </summary>
    public interface IAccountRepository
    {

        /// <summary>
        /// Gets all linked <see cref="Account"/>s from our database.
        /// </summary>
        /// <returns>List of <see cref="Account"/>s</returns>
        Task<IEnumerable<Account>> GetAll();

    }
}
