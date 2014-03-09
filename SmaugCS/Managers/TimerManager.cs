using System.Collections.Concurrent;
using System.Linq;
using System.Timers;
using Realm.Library.Common;

namespace SmaugCS.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TimerManager : GameSingleton
    {
        private static int _idSpace = 1;
        private static int GetNextId { get { return _idSpace++; } }

        private static TimerManager _instance;
        private static readonly object Padlock = new object();

        private readonly ConcurrentDictionary<int, CommonTimer> TimerTable;

        private TimerManager()
        {
            TimerTable = new ConcurrentDictionary<int, CommonTimer>();
        }

        ~TimerManager()
        {
            TimerTable.Values.ToList().ForEach(x => x.Dispose());
        }

        /// <summary>
        ///
        /// </summary>
        public static TimerManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new TimerManager());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public int AddTimer(double duration, ElapsedEventHandler callback)
        {
            int newId = GetNextId;

            CommonTimer newTimer = new CommonTimer(newId) {Interval = duration};
            newTimer.Elapsed += callback;

            TimerTable.GetOrAdd(newId, newTimer);
            newTimer.Start();
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timerId"></param>
        /// <returns></returns>
        public CommonTimer GetTimer(int timerId)
        {
            CommonTimer timer;
            TimerTable.TryGetValue(timerId, out timer);
            return timer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timerId"></param>
        public void DeleteTimer(int timerId)
        {
            CommonTimer timer;
            TimerTable.TryRemove(timerId, out timer);
            if (timer != null)
                timer.Dispose();
        }
    }
}
