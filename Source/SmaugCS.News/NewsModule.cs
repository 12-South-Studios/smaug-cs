using Ninject;
using Ninject.Modules;
using SmaugCS.DAL;
using SmaugCS.Logging;

namespace SmaugCS.News
{
    public class NewsModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<INewsRepository>().To<NewsRepository>()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("dbContext", Kernel.Get<IDbContext>());

            Kernel.Bind<INewsManager>().To<NewsManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("kernel", Kernel)
                .WithConstructorArgument("repository", Kernel.Get<INewsRepository>())
                .OnActivation(x => x.Initialize());
        }
    }
}
