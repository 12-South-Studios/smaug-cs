using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SmaugCS.Database.Tests
{
    [TestClass]
    public class SqlDatabaseSetup
    {
        [AssemblyInitialize]
        public static void InitializeAssembly(TestContext ctx)
        {
            SqlDatabaseTestClass.TestService.DeployDatabaseProject();
            SqlDatabaseTestClass.TestService.GenerateData();
        }
    }
}
