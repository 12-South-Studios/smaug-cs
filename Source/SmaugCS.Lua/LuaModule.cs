using Autofac;
using SmaugCS.Data.Interfaces;

namespace SmaugCS.Lua;

public class LuaModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<LuaManager>().As<ILuaManager>().SingleInstance();
  }
}