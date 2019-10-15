using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Poller
{

    /// <summary>
    /// Contains constants for our version.
    /// </summary>
    public static class Constants
    {

        /// <summary>
        /// Retrieve application version.
        /// </summary>
        public static Version ApplicationVersion => Assembly.GetEntryAssembly().GetName().Version;

        /// <summary>
        /// Retrieve application name.
        /// </summary>
        public static string ApplicationName => Assembly.GetEntryAssembly().GetName().Name;

    }
}
