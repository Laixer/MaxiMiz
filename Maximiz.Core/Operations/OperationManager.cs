using Maximiz.Core.Infrastructure.Commiting;
using Maximiz.Core.Operations.Abstraction;
using Maximiz.Core.Operations.Execution;
using Maximiz.Model.Entity;
using Maximiz.Model.Operations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Maximiz.Core.Operations
{

    /// <summary>
    /// Handles the creation of <see cref="Operation"/>s.
    /// </summary>
    public sealed class OperationManager : IOperationManager
    {

        private readonly ILogger logger;
        private readonly EntityAvailabilityChecker _entityAvailabilityChecker;
        private readonly IOperationHistoryCreator _operationHistoryCreator;
        private readonly IOperationCommitter _operationCommitter;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public OperationManager(ILoggerFactory loggerFactory,
            EntityAvailabilityChecker entityAvailabilityChecker,
            IOperationHistoryCreator operationHistoryCreator,
            IOperationCommitter operationCommitter)
        {
            _entityAvailabilityChecker = entityAvailabilityChecker ?? throw new ArgumentNullException(nameof(entityAvailabilityChecker));
            _operationHistoryCreator = operationHistoryCreator ?? throw new ArgumentNullException(nameof(operationHistoryCreator));
            _operationCommitter = operationCommitter ?? throw new ArgumentNullException(nameof(operationCommitter));

            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            logger = loggerFactory.CreateLogger(nameof(OperationManager));
        }

        /// <summary>
        /// Checks if we are allowed to create a new <see cref="Operation"/> by
        /// checking the entities it contains. If any of the entities is already
        /// being modified in another operation, this will throw an
        /// <see cref="InvalidOperationException"/>.
        /// TODO We should handle error display for this.
        /// </summary>
        /// <param name="entitiesModified"><see cref="Entity"/></param>
        /// <returns><see cref="Operation"/></returns>
        public async Task<Operation> CreateOperationAsync(IEnumerable<Entity<Guid>> entitiesModified)
        {
            if (entitiesModified == null) { throw new ArgumentNullException(nameof(entitiesModified)); }

            if (await _entityAvailabilityChecker.AreEntitiesAvailable(entitiesModified))
            {
                var operation = new Operation
                {
                    EntityIds = ExtractIds(entitiesModified)
                };
                return operation;
            }
            else
            {
                throw new InvalidOperationException($"Could not create new operation because specified entities are unavailable");
            }
        }

        /// <summary>
        /// TODO Replace with linq.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private IEnumerable<Guid> ExtractIds(IEnumerable<Entity<Guid>> entities)
        {
            var result = new List<Guid>();
            foreach (var entity in entities)
            {
                result.Add(entity.Id);
            }
            return result;
        }
    }
}
