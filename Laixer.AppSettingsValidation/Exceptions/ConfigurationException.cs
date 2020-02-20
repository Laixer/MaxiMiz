using System;

namespace Laixer.AppSettingsValidation.Exceptions
{

    /// <summary>
    /// <see cref="Exception"/> that indicates something went wrong in our 
    /// app settings configuration.
    /// </summary>
    public class ConfigurationException : Exception
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message"><see cref="Exception.Message"/></param>
        public ConfigurationException(string message) : base(message)
        {
        }

    }
}
