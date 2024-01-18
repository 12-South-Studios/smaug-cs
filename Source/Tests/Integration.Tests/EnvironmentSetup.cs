using Ninject;
using SmaugCS.DAL;
using System.Configuration;
using Xunit;

namespace Integration.Tests
{

    public class EnvironmentSetup
    {
        private static IKernel _kernel;

        public EnvironmentSetup()
        {
            _kernel = new StandardKernel(new DbContextModule(), new IntegrationModule());
        }

        ~EnvironmentSetup()
        {
            _kernel?.Dispose();
        }

        //[Fact(Skip = "Integration Test")]
        [Trait("Category", "Integration")]
        public void CreateAndInitializeDatabase()
        {
            DropDatabase("SmaugDbContext");
            Database.SetInitializer(new MigrateDatabaseToLatestVersion
                <SmaugDbContext, SmaugCS.DAL.Migrations.Configuration>());
            SmaugDatabaseSeeder.Kernel = _kernel;
            SmaugDatabaseSeeder.Seed();
        }

        private static void DropDatabase(string connectionStringName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            var builder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = builder.InitialCatalog;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                }
                catch (SqlException ex)
                {
                    return;
                }

                sqlConnection.ChangeDatabase("Master");
                var singleUserCommand = new SqlCommand(
                    $"alter database {databaseName} set single_user with rollback immediate",
                        sqlConnection);
                singleUserCommand.ExecuteNonQuery();

                var command = new SqlCommand($"drop database {databaseName}", sqlConnection);
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}