using Microsoft.Extensions.Options;

namespace Laixer.Library.Injection.Database
{

    /// <summary>
    /// Contains options for our database provider object.
    /// </summary>
    public class DatabaseProviderOptions : IOptionsBase
    {

        /// <summary>
        /// Contains the name of our database connection string.
        /// </summary>
        public string ConnectionStringName { get; set; }

    }
}
