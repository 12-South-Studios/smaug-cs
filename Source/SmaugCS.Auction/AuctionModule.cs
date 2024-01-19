using Autofac;
using Autofac.Features.AttributeFilters;
using Realm.Library.Common;

namespace SmaugCS.Auction
{
    public class AuctionModule : Module
    {
        private readonly Config.Configuration.Constants _constants;
        public AuctionModule(Config.Configuration.Constants constants)
        {
            _constants = constants;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommonTimer>().Named<ITimer>("AuctionPulseTimer")
                .OnActivated(x => x.Instance.Interval = _constants.AuctionPulseSeconds);

            builder.RegisterType<AuctionRepository>().As<IAuctionRepository>();
            builder.RegisterType<AuctionManager>().As<IAuctionManager>()
                .SingleInstance()
                .OnActivated(x => x.Instance.Initialize())
                .WithAttributeFiltering();
        }
    }
}
