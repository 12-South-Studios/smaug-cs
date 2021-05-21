using Realm.Library.Common;
using System.Timers;

namespace SmaugCS.Time
{
    public interface ITimerManager
    {
        int AddTimer(double duration, ElapsedEventHandler callback);
        CommonTimer GetTimer(int timerId);
        void DeleteTimer(int timerId);
    }
}
