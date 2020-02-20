using System;
using System.Threading;

namespace Poller.Scheduler
{

    /// <summary>
    /// Schedules our operations.
    /// </summary>
    public static class Scheduler
    {
        private static readonly Random rand = new Random();

        /// <summary>
        /// Schedule the timer for next interval.
        /// TODO This is still untested.
        /// </summary>
        /// <remarks>
        /// The scheduler will add a bias offset to the timer for a more 
        /// overal balanced scheme, if the function is configured to do so.
        /// </remarks>
        /// <param name="timer">Configurable timer.</param>
        /// <param name="timeSpan">Requested interval.</param>
        /// <param name="withBias">Optional timespan bias.</param>
        /// <returns>Timer object passed in.</returns>
        public static Timer ScheduleTimer(Timer timer, TimeSpan timeSpan, bool withBias = true)
        {
            var timerOffset = TimeSpan.FromSeconds(rand.Next(15, 45));

            timer.Change(withBias
                ? timeSpan.Add(timerOffset)
                : timeSpan, TimeSpan.FromMilliseconds(-1));
            return timer;
        }
    }
}
