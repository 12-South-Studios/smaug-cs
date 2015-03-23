﻿using Ninject;
using Ninject.Modules;
using Realm.Library.Common.Logging;
using Realm.Library.Network;
using SmallDBConnectivity;
using SmaugCS.Constants;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Interfaces;
using SmaugCS.Logging;
using SmaugCS.LuaHelpers;
using SmaugCS.Managers;
using SmaugCS.Repositories;
using SmaugCS.SpecFuns;

namespace SmaugCS
{
    public class SmaugModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ISmallDb>().To<SmallDb>();

            Kernel.Bind<ILookupManager>().To<LookupManager>().InSingletonScope();

            Kernel.Bind<ILuaManager>().To<LuaManager>().InSingletonScope()
                .WithConstructorArgument("logWrapper", Kernel.Get<ILogWrapper>())
                .WithConstructorArgument("path", GameConstants.DataPath);

            Kernel.Bind<ITcpUserRepository>().To<TcpUserRepository>();
            Kernel.Bind<ITcpServer>().To<TcpServer>().InSingletonScope()
                .WithConstructorArgument("logWrapper", Kernel.Get<ILogWrapper>())
                .WithConstructorArgument("repository", Kernel.Get<ITcpUserRepository>());

            Kernel.Bind<IDatabaseManager>().To<DatabaseManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>());

            Kernel.Bind<IGameManager>().To<GameManager>().InSingletonScope();

            Kernel.Bind<ICalendarManager>().To<CalendarManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("smallDb", Kernel.Get<ISmallDb>())
                .WithConstructorArgument("connection", SqlConnectionProvider.Connection)
                .WithConstructorArgument("gameManager", Kernel.Get<IGameManager>())
                .OnActivation(x => x.Initialize());

            Kernel.Bind<IInitializer>().To<LuaInitializer>().InSingletonScope()
                .Named("LuaInitializer")
                .OnActivation(x => x.InitializeLuaInjections(GameConstants.DataPath))
                .OnActivation(x => x.InitializeLuaFunctions());

            Kernel.Bind<ITimerManager>().To<TimerManager>().InSingletonScope();

            Kernel.Bind<IInstanceRepository<ObjectInstance>>().To<ObjInstanceRepository>();
            Kernel.Bind<IInstanceRepository<CharacterInstance>>().To<CharacterRepository>();
            Kernel.Bind<ITemplateRepository<MobTemplate>>().To<MobileRepository>();
            Kernel.Bind<ITemplateRepository<ObjectTemplate>>().To<ObjectRepository>();
            Kernel.Bind<ITemplateRepository<RoomTemplate>>().To<RoomRepository>();

            Kernel.Bind<ISpecFunHandler>().To<SpecFunHandler>();
        }
    }
}