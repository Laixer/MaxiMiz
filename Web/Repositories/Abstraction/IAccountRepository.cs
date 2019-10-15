using Maximiz.Model.Entity;
using System;

namespace Maximiz.Repositories.Abstraction
{

    /// <summary>
    /// Interface for an <see cref="Account"/> repository.
    /// </summary>
    public interface IAccountRepository : IEntityRepository<Account, Guid>
    {
    }

}
