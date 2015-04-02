using Ninject;
using Ninject.Modules;
using Realm.Library.Common.Logging;
using SmaugCS.Constants;
using SmaugCS.Data;

namespace SmaugCS.Lua
{
    public class LuaModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ILuaManager>().To<LuaManager>().InSingletonScope()
                .WithConstructorArgument("logWrapper", Kernel.Get<ILogWrapper>())
                .WithConstructorArgument("path", GameConstants.DataPath);
        }
    }
}
