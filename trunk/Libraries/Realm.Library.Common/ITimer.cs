using System;
using System.Timers;

namespace Realm.Library.Common
{
    /// <summary>
    /// Declares a timer interface
    /// </summary>
    public interface ITimer : IDisposable
    {
        void Start(double? interval = null);
        void Stop();
        double Interval { get; set; }
        bool Enabled { get; set; }
        event ElapsedEventHandler Elapsed;
    }
}