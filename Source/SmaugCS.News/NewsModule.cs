using Autofac;

namespace SmaugCS.News
{
    public class NewsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NewsRepository>().As<INewsRepository>();

            builder.RegisterType<NewsManager>().As<INewsManager>().SingleInstance()
                .OnActivated(x => x.Instance.Initialize());
        }
    }
}
