using Autofac;
using Autofac.Features.AttributeFilters;
using Realm.Library.Common;
using Realm.Library.Network;
using Realm.Library.Network.Formatters;
using Realm.Library.Network.Tcp;
using SmaugCS.Common;
using SmaugCS.Data.Interfaces;
using SmaugCS.MudProgs;
using SmaugCS.SpecFuns;
using System.Collections.Generic;
using System.Net;

namespace SmaugCS
{
    public class SmaugModule : Module
    {
        private readonly Config.Configuration.Settings _settings;
        private readonly Config.Configuration.Constants _constants;

        public SmaugModule(Config.Configuration.Settings settings, Config.Configuration.Constants constants)
        {
            _settings = settings;
            _constants = constants;
        }

        private IEnumerable<IFormatter> GetFormatters()
        {
            var formatters = new List<IFormatter>
            {
                new MxpFormatter(),
                new AnsiFormatter(),
                new TextFormatter()
            };
            return formatters;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Application>().AsSelf().SingleInstance();

            builder.RegisterType<CommonTimer>().Named<ITimer>("GameLoopTimer")
                .OnActivated(x => x.Instance.Interval = 1000f / _settings.Values.PulsesPerSecond);

            builder.RegisterType<LookupManager>().As<ILookupManager>().SingleInstance();

            builder.RegisterType<TcpUserRepository>().As<IUserRepository<string, TcpUser>>();
            builder.RegisterType<TcpServer>().As<INetworkServer>().SingleInstance()
                .WithParameter("settings", new NetworkSettings
                {
                    Port = _settings.Port,
                    IpAddress = IPAddress.Parse(_settings.Host).ToString()
                })
                .WithParameter("formatters", GetFormatters());

            builder.RegisterType<GameManager>().As<IGameManager>()
                .SingleInstance()
                .WithAttributeFiltering();
            builder.RegisterType<CalendarManager>().As<ICalendarManager>().SingleInstance()
                .OnActivated(x => x.Instance.Initialize());
            builder.RegisterType<LuaInitializer>().Named<IInitializer>("LuaInitializer").SingleInstance()
                .OnActivated(x =>
                {
                    x.Instance.Initialize();
                    x.Instance.InitializeLuaInjections($"{_constants.AppPath}/data/");
                    x.Instance.InitializeLuaFunctions();
                });

            builder.RegisterType<SpecFunHandler>().As<ISpecFunHandler>();
            builder.RegisterType<MudProgHandler>().As<IMudProgHandler>();
        }
    }
}
