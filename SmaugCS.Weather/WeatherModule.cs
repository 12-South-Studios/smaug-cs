﻿using Ninject;
using Ninject.Modules;
using SmallDBConnectivity;
using SmaugCS.Constants;
using SmaugCS.Interfaces;
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
                .WithConstructorArgument("smallDb", Kernel.Get<ISmallDb>())
                .WithConstructorArgument("connection", SqlConnectionProvider.Connection)
                .OnActivation(x => x.Initialize(Kernel.Get<IGameManager>().GameTime,
                    GameConstants.GetConstant<int>("WeatherWidth"),
                    GameConstants.GetConstant<int>("WeatherHeight")));
        }
    }
}
