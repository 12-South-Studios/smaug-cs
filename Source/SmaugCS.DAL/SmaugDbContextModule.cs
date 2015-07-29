using Ninject.Modules;
using SmaugCS.DAL.Interfaces;

namespace SmaugCS.DAL
{
    public class SmaugDbContextModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISmaugDbContext>().To<SmaugDbContext>().InSingletonScope();
        }
    }
}
