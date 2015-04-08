using Infrastructure.Data;
using Ninject;
using Ninject.Modules;
using SmaugCS.Logging;

namespace SmaugCS.News
{
    public class NewsModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<INewsRepository>().To<NewsRepository>()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("repository", Kernel.Get<IRepository>());

            Kernel.Bind<INewsManager>().To<NewsManager>().InSingletonScope()
                .WithConstructorArgument("logManager", Kernel.Get<ILogManager>())
                .WithConstructorArgument("kernel", Kernel)
                .WithConstructorArgument("repository", Kernel.Get<INewsRepository>())
                .OnActivation(x => x.Initialize());
        }
    }
}
