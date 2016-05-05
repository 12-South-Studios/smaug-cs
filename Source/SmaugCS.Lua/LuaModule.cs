using Ninject;
using Ninject.Modules;
using Realm.Library.Common.Logging;
using SmaugCS.Data.Interfaces;

namespace SmaugCS.Lua
{
    public class LuaModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ILuaManager>().To<LuaManager>().InSingletonScope()
                .WithConstructorArgument("logWrapper", Kernel.Get<ILogWrapper>());
        }
    }
}
