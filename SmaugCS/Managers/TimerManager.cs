using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Realm.Library.Common;
using Realm.Library.Common.Objects;

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
            CommonTimer newTimer = new CommonTimer {Interval = duration};
            newTimer.Elapsed += callback;

            int newId = GetNextId;
            TimerTable.GetOrAdd(newId, newTimer);
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
