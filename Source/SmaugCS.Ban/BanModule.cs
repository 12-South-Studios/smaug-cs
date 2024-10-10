using Autofac;
using Autofac.Features.AttributeFilters;
using Library.Common;

namespace SmaugCS.Ban;

public class BanModule(Config.Configuration.Constants constants) : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<CommonTimer>().Named<ITimer>("BanExpireTimer")
      .OnActivated(x => x.Instance.Interval = constants.BanExpireFrequencyMS);

    builder.RegisterType<BanRepository>().As<IBanRepository>();
    builder.RegisterType<BanManager>().As<IBanManager>()
      .SingleInstance()
      .OnActivated(x => x.Instance.Initialize())
      .WithAttributeFiltering();
  }
}