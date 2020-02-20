﻿
namespace Maximiz.Infrastructure.Database
{

    /// <summary>
    /// Contains all options for our <see cref="NpgsqlDatabaseProvider"/>.
    /// </summary>
    public sealed class NpgsqlDatabaseProviderOptions
    {

        /// <summary>
        /// The name of the connection string in the IConfigurations file.
        /// </summary>
        public string ConnectionStringName { get; set; }

    }
}
