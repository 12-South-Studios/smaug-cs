using Autofac;
using Autofac.Features.AttributeFilters;
using Realm.Library.Common;

namespace SmaugCS.Logging
{
    public class LoggingModule : Module
    {
        private readonly Config.Configuration.Constants _constants;
        public LoggingModule(Config.Configuration.Constants constants)
        {
            _constants = constants;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommonTimer>().Named<ITimer>("LogDumpTimer")
                .OnActivated(x => x.Instance.Interval = _constants.LogDumpFrequencyMS);

            builder.RegisterType<LogManager>().As<ILogManager>()
                .SingleInstance()
                .WithAttributeFiltering();
        }
    }
}
