﻿using Ninject;
using Ninject.Modules;
using Realm.Library.Common;
using SmallDBConnectivity;
using SmaugCS.Constants;
using SmaugCS.Logging;

namespace SmaugCS.Auction
{
    public class AuctionModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ITimer>().To<CommonTimer>().Named("AuctionPulseTimer")
                .OnActivation(x => x.Interval = GameConstants.GetConstant<int>("AuctionPulseSeconds"));

            Kernel.Bind<IAuctionRepository>().To<AuctionRepository>()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("smallDb", Kernel.Get<ISmallDb>())
                .WithConstructorArgument("connection", SqlConnectionProvider.Connection);

            Kernel.Bind<IAuctionManager>().To<AuctionManager>().InSingletonScope()
                .WithConstructorArgument("kernel", Kernel)
                .WithConstructorArgument("timer", Kernel.Get<ITimer>("AuctionPulseTimer"))
                .WithConstructorArgument("repository", Kernel.Get<IAuctionRepository>())
                .OnActivation(x => x.Initialize());
        }
    }
}