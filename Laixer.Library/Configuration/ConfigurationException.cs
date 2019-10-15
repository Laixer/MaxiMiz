using System;

namespace Laixer.Library.Configuration.Exceptions
{

    /// <summary>
    /// Exception to indicate something's wrong with our applications configuration.
    /// TODO Move to more logical location.
    /// </summary>
    public class ConfigurationException : Exception
    {

        /// <summary>
        /// Constructor with message.
        /// </summary>
        /// <param name="message">The message</param>
        public ConfigurationException(string message) : base(message) { }

        /// <summary>
        /// Constructor with message and inner exception.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="innerException">The inner exception</param>
        public ConfigurationException(string message, Exception innerException) : base(message, innerException) { }

    }
}
