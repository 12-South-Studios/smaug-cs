using Autofac;

namespace SmaugCS.Clans;

public class ClanModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<ClanRepository>().As<IClanRepository>();
    builder.RegisterType<ClanManager>().As<IClanManager>().SingleInstance()
      .OnActivated(x => x.Instance.Initialize());
  }
}