using System.Diagnostics.CodeAnalysis;
using System.Timers;

namespace Realm.Library.Common
{
    /// <summary>
    /// Custom timer class implements ITimer and wraps
    /// an existing System.Timer so that it can be more easily injected.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class CommonTimer : ITimer
    {
        private readonly Timer _timer = new Timer();

        #region ITimer

        /// <summary>
        /// Starts the timer with the given interval
        /// </summary>
        /// <param name="interval"></param>
        public void Start(double? interval = null)
        {
            _timer.Interval = interval.IsNotNull() ? interval.GetValueOrDefault(Interval) : Interval;
            _timer.Start();
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }

        /// <summary>
        /// Gets the interval
        /// </summary>
        public double Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        /// <summary>
        /// Gets the elapsed event delegate
        /// </summary>
        public event ElapsedEventHandler Elapsed
        {
            add { _timer.Elapsed += value; }
            remove { _timer.Elapsed -= value; }
        }

        /// <summary>
        /// Is this timer enabled
        /// </summary>
        public bool Enabled
        {
            get { return _timer.Enabled; }
            set { _timer.Enabled = value; }
        }

        #endregion ITimer

        #region IDisposable

        /// <summary>
        /// Overrides the base Dispose to make this object disposable
        /// </summary>
        public void Dispose()
        {
            if (_timer.IsNotNull())
                _timer.Dispose();
        }

        #endregion IDisposable
    }
}