using Ninject;
using Ninject.Modules;
using Realm.Library.Common.Logging;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;

namespace SmaugCS.Repository
{
    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IRepositoryManager>().To<RepositoryManager>().InSingletonScope()
                .WithConstructorArgument("kernel", Kernel)
                .WithConstructorArgument("logWrapper", Kernel.Get<ILogWrapper>());

            Kernel.Bind<IInstanceRepository<ObjectInstance>>().To<ObjInstanceRepository>();
            Kernel.Bind<IInstanceRepository<CharacterInstance>>().To<CharacterRepository>();
            Kernel.Bind<ITemplateRepository<MobileTemplate>>().To<MobileRepository>();
            Kernel.Bind<ITemplateRepository<ObjectTemplate>>().To<ObjectRepository>();
            Kernel.Bind<ITemplateRepository<RoomTemplate>>().To<RoomRepository>();
        }
    }
}
