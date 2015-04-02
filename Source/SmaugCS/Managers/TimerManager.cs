using System.Collections.Concurrent;
using System.Linq;
using System.Timers;
using Ninject;
using Realm.Library.Common;
using SmaugCS.Interfaces;

namespace SmaugCS.Managers
{
    public sealed class TimerManager : ITimerManager
    {
        private static int _idSpace = 1;
        private static int GetNextId { get { return _idSpace++; } }
        private static IKernel _kernel;

        private readonly ConcurrentDictionary<int, CommonTimer> TimerTable;

        public TimerManager(IKernel kernel)
        {
            _kernel = kernel;
            TimerTable = new ConcurrentDictionary<int, CommonTimer>();
        }

        ~TimerManager()
        {
            TimerTable.Values.ToList().ForEach(x => x.Dispose());
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

            TimerTable.GetOrAdd(newId, newTimer);
            newTimer.Start();
            return newId;
        }

        public CommonTimer GetTimer(int timerId)
        {
            CommonTimer timer;
            TimerTable.TryGetValue(timerId, out timer);
            return timer;
        }

        public void DeleteTimer(int timerId)
        {
            CommonTimer timer;
            TimerTable.TryRemove(timerId, out timer);
            if (timer != null)
                timer.Dispose();
        }
    }
}
