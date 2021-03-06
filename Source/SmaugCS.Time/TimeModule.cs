﻿using Ninject.Modules;

namespace SmaugCS.Time
{
    public class TimeModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ITimerManager>().To<TimerManager>().InSingletonScope();
        }
    }
}
