using Autofac;
using SmaugCS.Data.Interfaces;

namespace SmaugCS.Weather
{
    public class WeatherModule : Module
    {
        private readonly Config.Configuration.Constants _constants;
        public WeatherModule(Config.Configuration.Constants constants)
        {
            _constants = constants;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WeatherManager>().As<IWeatherManager>().SingleInstance()
                .OnActivated(x => x.Instance.Initialize(
                    x.Context.Resolve<IGameManager>().GameTime, 
                    _constants.WeatherWidth,
                    _constants.WeatherHeight));
        }
    }
}
