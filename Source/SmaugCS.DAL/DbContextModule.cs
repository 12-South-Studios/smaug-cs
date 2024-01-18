using Ninject.Modules;

namespace SmaugCS.DAL
{
    public class DbContextModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDbContext>().To<DbContext>().InSingletonScope();
        }
    }
}
