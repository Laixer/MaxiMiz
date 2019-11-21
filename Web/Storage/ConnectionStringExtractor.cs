using System;
using System.Collections.Generic;

namespace Maximiz.Storage
{

    /// <summary>
    /// Extracts parameters from a connection string.
    /// TODO Place in another package.
    /// </summary>
    public sealed class ConnectionStringExtractor
    {

        /// <summary>
        /// Connection string as a whole.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// All extracted key value pairs from the connection string.
        /// </summary>
        private readonly Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connetion string to analyze</param>
        public ConnectionStringExtractor(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) { throw new ArgumentException("Connection string can not be null"); }
            _connectionString = connectionString;
            ExtractKeyValuePairs();
        }

        /// <summary>
        /// Extracts all the key value pairs from our connection string.
        /// </summary>
        private void ExtractKeyValuePairs()
        {
            var parts = _connectionString.Split(';');
            foreach (var part in parts)
            {
                var keyAndValue = part.Split('=', 2);
                var key = keyAndValue[0];
                var value = keyAndValue[1];
                keyValuePairs.Add(key, value);
            }
        }

        /// <summary>
        /// Checks if our connection string contains a key.
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>True if the connection string contains the key</returns>
        public bool HasKey(string key)
            => keyValuePairs.ContainsKey(key);

        /// <summary>
        /// Gets a value based on some key.
        /// </summary>
        /// <remarks>
        /// This will throw if the key does not exist.
        /// </remarks>
        /// <param name="key">The key to check</param>
        /// <returns>The corresponding value</returns>
        public string GetValue(string key)
            => keyValuePairs[key];
    }
}
