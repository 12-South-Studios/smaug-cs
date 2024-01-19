using Autofac;

namespace SmaugCS.Time
{
    public class TimeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TimerManager>().As<ITimerManager>().SingleInstance();
        }
    }
}
