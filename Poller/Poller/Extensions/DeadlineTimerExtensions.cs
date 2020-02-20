using System;
using Poller.Scheduler;

namespace Poller.Extensions
{
    public static class DeadlineTimerExtensions
    {
        /// <summary>
        /// Reset the deadline timer with the new interval.
        /// </summary>
        /// <param name="timer">Deadline timer instance.</param>
        /// <param name="timeSpan">Deadline span.</param>
        /// <returns>True if the timer was successfully updated; otherwise, false.</returns>
        public static bool Reset(this DeadlineTimer timer, TimeSpan timeSpan)
            => timer.Change(timeSpan, TimeSpan.FromMilliseconds(-1));
    }
}
