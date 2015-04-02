﻿using Ninject;
using Ninject.Modules;
using SmaugCS.Constants;
using SmaugCS.DAL.Interfaces;
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
                .WithConstructorArgument("dbContext", Kernel.Get<ISmaugDbContext>())
                .OnActivation(x => x.Initialize(Kernel.Get<IGameManager>().GameTime,
                    GameConstants.GetConstant<int>("WeatherWidth"),
                    GameConstants.GetConstant<int>("WeatherHeight")));
        }
    }
}