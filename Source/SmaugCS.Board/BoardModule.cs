using Autofac;

namespace SmaugCS.Board;

public class BoardModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<BoardRepository>().As<IBoardRepository>();
    builder.RegisterType<BoardManager>().As<IBoardManager>().SingleInstance()
      .OnActivated(x => x.Instance.Initialize());
  }
}