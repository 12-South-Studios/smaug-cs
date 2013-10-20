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
        ///
        /// </summary>
        /// <param name="interval"></param>
        void Start(double? interval = null);

        /// <summary>
        ///
        /// </summary>
        void Stop();

        /// <summary>
        ///
        /// </summary>
        double Interval { get; set; }

        /// <summary>
        ///
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        ///
        /// </summary>
        event ElapsedEventHandler Elapsed;
    }
}