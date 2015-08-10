using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using Ninject;
using NUnit.Framework;
using SmaugCS.DAL;

namespace Integration.Tests
{
    [TestFixture]
    public class EnvironmentSetup
    {
        private static IKernel _kernel;

        [SetUp]
        public static void Initialize()
        {
            _kernel = new StandardKernel(new SmaugDbContextModule(), new IntegrationModule());
        }

        [TearDown]
        public static void CleanUp()
        {
            if (_kernel != null)
                _kernel.Dispose();
        }

        [Test]
        [Category("Integration")]
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
                var singleUserCommand = new SqlCommand(string.Format("alter database {0} set single_user with rollback immediate", databaseName),
                        sqlConnection);
                singleUserCommand.ExecuteNonQuery();

                var command = new SqlCommand(string.Format("drop database {0}", databaseName), sqlConnection);
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}