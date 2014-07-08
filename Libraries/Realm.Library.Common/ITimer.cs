using System;
using System.Timers;

namespace Realm.Library.Common
{
    /// <summary>
    /// Declares a timer interface
    /// </summary>
    public interface ITimer : IDisposable
    {
        /// <summary>
        /// Starts the timer with the given interval
        /// </summary>
        /// <param name="interval"></param>
        void Start(double? interval = null);

        /// <summary>
        /// Stops the timer
        /// </summary>
        void Stop();

        /// <summary>
        /// Gets or Sets the Interval
        /// </summary>
        double Interval { get; set; }

        /// <summary>
        /// Gets if the Timer is enabled
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Elapsed event handler
        /// </summary>
        event ElapsedEventHandler Elapsed;
    }
}