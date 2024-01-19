using Autofac;

namespace SmaugCS.DAL
{
    public class DbContextModule : Module
    {
        private readonly string _dbConnectionString;
        public DbContextModule(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbContext>().As<IDbContext>()
                .WithParameter("connectionString", _dbConnectionString);
        }
    }
}
