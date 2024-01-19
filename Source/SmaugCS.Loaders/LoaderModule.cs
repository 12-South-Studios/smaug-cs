using Autofac;
using SmaugCS.Common;
using SmaugCS.Loaders.Loaders;
using System.Collections.Generic;

namespace SmaugCS.Loaders
{
    public class LoaderModule : Module
    {
        private IEnumerable<IBaseLoader> GetLoaders()
        {
            var list = new List<IBaseLoader>();

            return list;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<AreaListLoader>().Named<IBaseLoader>("AreaLoader");
            //builder.RegisterType<ClanLoader>().Named<IBaseLoader>("ClanLoader");
            //builder.RegisterType<ClassLoader>().Named<IBaseLoader>("ClassLoader");
            //builder.RegisterType<CouncilLoader>().Named<IBaseLoader>("CouncilLoader");
            //builder.RegisterType<DeityListLoader>().Named<IBaseLoader>("DeityListLoader");
            //builder.RegisterType<LanguageLoader>().Named<IBaseLoader>("LanguageLoader");
            //builder.RegisterType<RaceLoader>().Named<IBaseLoader>("RaceLoader");
            builder.RegisterType<LoaderInitializer>().Named<IInitializer>("LoaderInitializer")
                .SingleInstance()
                .WithParameter("loaders", GetLoaders())
                .OnActivated(x => x.Instance.Initialize());
        }
    }
}
