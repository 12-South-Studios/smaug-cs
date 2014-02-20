using System.Diagnostics.CodeAnalysis;
using System.Timers;

namespace Realm.Library.Common
{
    /// <summary>
    /// Custom timer class implements ITimer and wraps an existing System.Timer so that it can be more easily injected.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class CommonTimer : ITimer
    {
        private readonly Timer _timer = new Timer();

        public CommonTimer() {}

        public CommonTimer(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        #region ITimer

        public void Start(double? interval = null)
        {
            _timer.Interval = interval.IsNotNull() ? interval.GetValueOrDefault(Interval) : Interval;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public double Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        public event ElapsedEventHandler Elapsed
        {
            add { _timer.Elapsed += value; }
            remove { _timer.Elapsed -= value; }
        }

        public bool Enabled
        {
            get { return _timer.Enabled; }
            set { _timer.Enabled = value; }
        }

        #endregion ITimer

        #region IDisposable

        public void Dispose()
        {
            if (_timer.IsNotNull())
                _timer.Dispose();
        }

        #endregion IDisposable
    }
}