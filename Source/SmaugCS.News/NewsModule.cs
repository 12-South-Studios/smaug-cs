using Ninject.Modules;

namespace SmaugCS.News
{
    public class NewsModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<INewsManager>().To<NewsManager>().InSingletonScope();
        }
    }
}
