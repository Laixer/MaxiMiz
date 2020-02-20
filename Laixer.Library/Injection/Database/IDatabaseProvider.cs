using System.Data;

namespace Laixer.Library.Injection.Database
{

    /// <summary>
    /// Contract for a database connection provider.
    /// TODO This is used in the poller as well, generalize!
    /// </summary>
    public interface IDatabaseProvider
    {

        /// <summary>
        /// Provides a new connection to our database.
        /// </summary>
        /// <returns>The connection</returns>
        IDbConnection GetConnectionScope();

    }
}
