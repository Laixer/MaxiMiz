using Maximiz.Model.Entity;
using Microsoft.Extensions.Logging;
using Poller.Database;
using System;
using System.Threading.Tasks;

namespace Poller.Taboola.Traffic
{
    /// <summary>
    /// Responsible for performing update and delete operations in our own 
    /// database. This is a partial class.
    /// </summary>
    internal partial class CrudInternal
    {

        /// <summary>
        /// Logging interface.
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// The database connection provider object.
        /// </summary>
        private readonly DbProvider _dbProvider;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="dbProvider">Database connection provider object</param>
        public CrudInternal(ILogger logger, DbProvider dbProvider)
        {
            _logger = logger;
            _dbProvider = dbProvider;
        }


    }
}
