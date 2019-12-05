using System.Data;

namespace Maximiz.Infrastructure.Database
{

    /// <summary>
    /// Contract for providing database connections.
    /// </summary>
    public interface IDatabaseProvider
    {

        /// <summary>
        /// Gets a database connection scope.
        /// </summary>
        /// <returns><see cref="IDbConnection"/></returns>
        IDbConnection GetConnectionScope();

    }
}
