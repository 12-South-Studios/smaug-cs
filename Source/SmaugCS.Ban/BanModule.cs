using Autofac;
using Autofac.Features.AttributeFilters;
using Realm.Library.Common;

namespace SmaugCS.Ban
{
    public class BanModule : Module
    {
        private readonly Config.Configuration.Constants _constants;
        public BanModule(Config.Configuration.Constants constants)
        {
            _constants = constants;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommonTimer>().Named<ITimer>("BanExpireTimer")
                .OnActivated(x => x.Instance.Interval = _constants.BanExpireFrequencyMS);

            builder.RegisterType<BanRepository>().As<IBanRepository>();
            builder.RegisterType<BanManager>().As<IBanManager>()
                .SingleInstance()
                .OnActivated(x => x.Instance.Initialize())
                .WithAttributeFiltering();
        }
    }
}
