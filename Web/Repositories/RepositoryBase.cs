using Maximiz.Database;
using Maximiz.Repositories.Abstraction;
using System.Threading;

namespace Maximiz.Repositories
{

    /// <summary>
    /// Used as a base for all our <see cref="IRepository<Entity>"/> implementations.
    /// This handles our data store access dependency injection.
    /// </summary>
    internal abstract class RepositoryBase
    {

        /// <summary>
        /// Indicates how long we can last without re-updating a certain list
        /// upon list request.
        /// TODO Explain better.
        /// </summary>
        protected const int MaxIntervalWithoutUpdatingSeconds = 300;

        /// <summary>
        /// Used to perform internal read operations.
        /// </summary>
        protected ICrudInternalWebClient crudInternal;

        /// <summary>
        /// Provides <see cref="CancellationToken"/>s for us.
        /// </summary>
        protected CancellationTokenSource source;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="crudInternalWebClient"><see cref="CrudInternalWebClient"/></param>
        public RepositoryBase(ICrudInternalWebClient crudInternalWebClient)
        {
            crudInternal = crudInternalWebClient;
            source = new CancellationTokenSource();
        }

    }
}
