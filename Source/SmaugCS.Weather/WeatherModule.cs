using Autofac;
using SmaugCS.Data.Interfaces;

namespace SmaugCS.Weather;

public class WeatherModule(Config.Configuration.Constants constants) : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<WeatherManager>().As<IWeatherManager>().SingleInstance()
      .OnActivated(x => x.Instance.Initialize(
        x.Context.Resolve<IGameManager>().GameTime,
        constants.WeatherWidth,
        constants.WeatherHeight));
  }
}