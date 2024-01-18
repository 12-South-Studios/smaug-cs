using Ninject;
using Ninject.Modules;
using SmaugCS.Constants.Constants;
using SmaugCS.DAL;
using SmaugCS.Data.Interfaces;
using SmaugCS.Logging;

namespace SmaugCS.Weather
{
    public class WeatherModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IWeatherManager>().To<WeatherManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("kernel", Kernel)
                .WithConstructorArgument("dbContext", Kernel.Get<IDbContext>())
                .OnActivation(x => x.Initialize(Kernel.Get<IGameManager>().GameTime,
                    GameConstants.GetConstant<int>("WeatherWidth"),
                    GameConstants.GetConstant<int>("WeatherHeight")));
        }
    }
}
