using Ninject;
using Realm.Library.Common;
using System.Collections.Concurrent;
using System.Linq;
using System.Timers;

namespace SmaugCS.Time
{
    public sealed class TimerManager : ITimerManager
    {
        private static int _idSpace = 1;
        private static int GetNextId => _idSpace++;
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

        public static ITimerManager Instance => _kernel.Get<ITimerManager>();

        public int AddTimer(double duration, ElapsedEventHandler callback)
        {
            var newId = GetNextId;

            var newTimer = new CommonTimer(newId) { Interval = duration };
            newTimer.Elapsed += callback;

            _timerTable.GetOrAdd(newId, newTimer);
            newTimer.Start();
            return newId;
        }

        public CommonTimer GetTimer(int timerId)
        {
            _timerTable.TryGetValue(timerId, out CommonTimer timer);
            return timer;
        }

        public void DeleteTimer(int timerId)
        {
            _timerTable.TryRemove(timerId, out CommonTimer timer);
            timer?.Dispose();
        }
    }
}
