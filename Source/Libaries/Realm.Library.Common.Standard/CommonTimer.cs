using System.Diagnostics.CodeAnalysis;
using System.Timers;
using Realm.Library.Common.Objects;

namespace Realm.Library.Common
{
    /// <summary>
    /// Custom timer class implements ITimer and wraps an existing System.Timer so that it can be more easily injected.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CommonTimer : ITimer
    {
        private readonly Timer _timer = new Timer();

        /// <summary>
        /// 
        /// </summary>
        public CommonTimer() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public CommonTimer(int id)
        {
            Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; private set; }

        #region ITimer
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        public void Start(double? interval = null)
        {
            _timer.Interval = interval.IsNotNull() ? interval.GetValueOrDefault(Interval) : Interval;
            _timer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        public double Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public event ElapsedEventHandler Elapsed
        {
            add { _timer.Elapsed += value; }
            remove { _timer.Elapsed -= value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Enabled
        {
            get { return _timer.Enabled; }
            set { _timer.Enabled = value; }
        }

        #endregion ITimer

        #region IDisposable
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_timer.IsNotNull())
                _timer.Dispose();
        }

        #endregion IDisposable
    }
}