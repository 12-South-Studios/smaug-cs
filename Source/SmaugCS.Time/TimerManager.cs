using System.Collections.Concurrent;
using System.Linq;
using System.Timers;
using Ninject;
using Realm.Library.Common;

namespace SmaugCS.Time
{
    public sealed class TimerManager : ITimerManager
    {
        private static int _idSpace = 1;
        private static int GetNextId { get { return _idSpace++; } }
        private static IKernel _kernel;

        private readonly ConcurrentDictionary<int, CommonTimer> _timerTable;

        public TimerManager(IKernel kernel)
        {
            _kernel = kernel;
            _timerTable = new ConcurrentDictionary<int, CommonTimer>();
        }

        ~TimerManager()
        {
            _timerTable.Values.ToList().ForEach(x => x.Dispose());
        }

        public static ITimerManager Instance
        {
            get { return _kernel.Get<ITimerManager>(); }
        }

        public int AddTimer(double duration, ElapsedEventHandler callback)
        {
            var newId = GetNextId;

            var newTimer = new CommonTimer(newId) {Interval = duration};
            newTimer.Elapsed += callback;

            _timerTable.GetOrAdd(newId, newTimer);
            newTimer.Start();
            return newId;
        }

        public CommonTimer GetTimer(int timerId)
        {
            CommonTimer timer;
            _timerTable.TryGetValue(timerId, out timer);
            return timer;
        }

        public void DeleteTimer(int timerId)
        {
            CommonTimer timer;
            _timerTable.TryRemove(timerId, out timer);
            if (timer != null)
                timer.Dispose();
        }
    }
}
