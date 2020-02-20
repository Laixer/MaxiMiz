using Maximiz.Core.Infrastructure.Repositories;
using Maximiz.Infrastructure.Database;
using Maximiz.Model.Entity;
using Maximiz.Model.Operations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maximiz.Infrastructure.Repositories
{

    public sealed class OperationRepository : RepositoryBase<Operation>, IOperationRepository
    {

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public OperationRepository(IDatabaseProvider databaseProvider)
            : base(databaseProvider) { }

        public override Task<Operation> GetAsync(Guid id)
            => RepositorySharedFunctions.GetAsync<Operation>(_databaseProvider, id, (x) => x.Id);

        public Task<IEnumerable<Entity>> GetEntitiesAfterModificationAsync(Operation operation)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Entity>> GetEntitiesBeforeModificationAsync(Operation operation)
        {
            throw new NotImplementedException();
        }

        public Task<Entity> GetEntityAfterModificationAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Entity> GetEntityBeforeModificationAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsOperationFinishedAsync(Operation operation)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsOperationLockedAsync(Operation operation)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsOperationLockedByIdAsync(Operation operation, Guid lockId)
        {
            throw new NotImplementedException();
        }
    }
}
