using Autofac;
using Autofac.Features.AttributeFilters;
using Library.Common;

namespace SmaugCS.Auction;

public class AuctionModule(Config.Configuration.Constants constants) : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<CommonTimer>().Named<ITimer>("AuctionPulseTimer")
      .OnActivated(x => x.Instance.Interval = constants.AuctionPulseSeconds);

    builder.RegisterType<AuctionRepository>().As<IAuctionRepository>();
    builder.RegisterType<AuctionManager>().As<IAuctionManager>()
      .SingleInstance()
      .OnActivated(x => x.Instance.Initialize())
      .WithAttributeFiltering();
  }
}