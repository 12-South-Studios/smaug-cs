using Autofac;

namespace SmaugCS.DAL;

public class DbContextModule(string dbConnectionString) : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<DbContext>().As<IDbContext>()
      .WithParameter("connectionString", dbConnectionString);
  }
}