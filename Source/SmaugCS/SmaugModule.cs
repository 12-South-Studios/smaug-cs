using Autofac;
using Autofac.Features.AttributeFilters;
using Library.Common;
using Library.Network;
using Library.Network.Formatters;
using Library.Network.Tcp;
using SmaugCS.Common;
using SmaugCS.Data.Interfaces;
using SmaugCS.MudProgs;
using SmaugCS.SpecFuns;
using System.Collections.Generic;
using System.Net;
using SmaugCS.Interfaces;
using SmaugCS.LuaHelpers;
using SmaugCS.Managers;
using System;

namespace SmaugCS;

public class SmaugModule(Config.Configuration.Settings settings, Config.Configuration.Constants constants)
  : Module
{
  private static IEnumerable<IFormatter> GetFormatters()
  {
    List<IFormatter> formatters =
    [
      new MxpFormatter(),
      new AnsiFormatter(),
      new TextFormatter()
    ];
    return formatters;
  }

  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<Application>().AsSelf().SingleInstance();

    builder.RegisterType<CommonTimer>().Named<ITimer>("GameLoopTimer")
      .OnActivated(x => x.Instance.Interval = 1000f / settings.Values.PulsesPerSecond);

    builder.RegisterType<LookupManager>().As<ILookupManager>().SingleInstance();

    builder.RegisterType<TcpUserRepository>().As<IUserRepository<string, TcpUser>>();
    builder.RegisterType<TcpServer>().As<INetworkServer>().SingleInstance()
      .WithParameter("settings", new NetworkSettings
      {
        Port = settings.Port,
        IpAddress = IPAddress.Parse(settings.Host).ToString()
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
        x.Instance.InitializeLuaInjections($"{constants.AppPath}/data/");
        x.Instance.InitializeLuaFunctions();
      });

    builder.RegisterType<SpecFunHandler>().As<ISpecFunHandler>();
    builder.RegisterType<MudProgHandler>().As<IMudProgHandler>();
  }
}