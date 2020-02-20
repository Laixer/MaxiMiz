
using System;

namespace Maximiz.Core.Operations
{

    /// <summary>
    /// Contains constants about <see cref="Operation"/> processing.
    /// </summary>
    public static class OperationConstants
    {

        /// <summary>
        /// Indicates how many times we may enter the failed state before exiting.
        /// </summary>
        public static int MaxFailureCount { get; } = 1;

        /// <summary>
        /// Indicates how long an <see cref="Operation"/> may take before forcing
        /// failure, attempt restore and exit.
        /// </summary>
        public static TimeSpan MaxOperationTime { get; } = new TimeSpan(hours: 0, minutes: 1, seconds: 30);

    }
}
