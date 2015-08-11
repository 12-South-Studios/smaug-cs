using Ninject.Modules;
using SmaugCS.Common;
using SmaugCS.Loaders.Loaders;

namespace SmaugCS.Loaders
{
    public class LoaderModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<BaseLoader>().To<AreaListLoader>().Named("AreaLoader");
            Kernel.Bind<BaseLoader>().To<ClanLoader>().Named("ClanLoader");
            Kernel.Bind<BaseLoader>().To<ClassLoader>().Named("ClassLoader");
            Kernel.Bind<BaseLoader>().To<CouncilLoader>().Named("CouncilLoader");
            Kernel.Bind<BaseLoader>().To<DeityListLoader>().Named("DeityLoader");
            Kernel.Bind<BaseLoader>().To<LanguageLoader>().Named("LanguageLoader");
            Kernel.Bind<BaseLoader>().To<RaceLoader>().Named("RaceLoader");
            Kernel.Bind<IInitializer>().To<LoaderInitializer>().InSingletonScope()
                .Named("LoaderInitializer")
                .OnActivation(x => x.Initialize());
        }
    }
}
