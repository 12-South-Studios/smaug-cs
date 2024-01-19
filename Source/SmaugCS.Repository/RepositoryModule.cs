using Autofac;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;

namespace SmaugCS.Repository
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RepositoryManager>().As<IRepositoryManager>().SingleInstance();

            builder.RegisterType<ObjInstanceRepository>().As<IInstanceRepository<ObjectInstance>>();
            builder.RegisterType<CharacterRepository>().As<IInstanceRepository<CharacterInstance>>();
            builder.RegisterType<MobileRepository>().As<ITemplateRepository<MobileTemplate>>();
            builder.RegisterType<ObjectRepository>().As<ITemplateRepository<ObjectTemplate>>();
            builder.RegisterType<RoomRepository>().As<ITemplateRepository<RoomTemplate>>();
        }
    }
}
