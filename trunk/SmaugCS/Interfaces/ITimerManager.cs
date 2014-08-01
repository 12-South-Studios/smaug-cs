﻿using System.Timers;
using Realm.Library.Common;

namespace SmaugCS.Interfaces
{
    public interface ITimerManager
    {
        int AddTimer(double duration, ElapsedEventHandler callback);
        CommonTimer GetTimer(int timerId);
        void DeleteTimer(int timerId);
    }
}
