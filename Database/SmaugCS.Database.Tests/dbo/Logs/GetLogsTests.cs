using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SmaugCS.Database.Tests
{
    [TestClass]
    public class GetLogsTests : SqlDatabaseTestClass
    {
        private SqlDatabaseTestActions GetLogs_RowCount_TestData;

        public GetLogsTests()
        {
            InitializeComponent();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeTest();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            CleanupTest();
        }

        #region Designer support code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SqlDatabaseTestAction GetLogs_RowCount_Test_TestAction;
            ComponentResourceManager resources = new ComponentResourceManager(typeof(GetLogsTests));
            RowCountCondition rowCountCondition1;
            SqlDatabaseTestAction GetLogs_RowCount_Test_PretestAction;
            SqlDatabaseTestAction GetLogs_RowCount_Test_PosttestAction;
            GetLogs_RowCount_TestData = new SqlDatabaseTestActions();
            GetLogs_RowCount_Test_TestAction = new SqlDatabaseTestAction();
            rowCountCondition1 = new RowCountCondition();
            GetLogs_RowCount_Test_PretestAction = new SqlDatabaseTestAction();
            GetLogs_RowCount_Test_PosttestAction = new SqlDatabaseTestAction();
            // 
            // GetLogs_RowCount_Test_TestAction
            // 
            GetLogs_RowCount_Test_TestAction.Conditions.Add(rowCountCondition1);
            resources.ApplyResources(GetLogs_RowCount_Test_TestAction, "GetLogs_RowCount_Test_TestAction");
            // 
            // rowCountCondition1
            // 
            rowCountCondition1.Enabled = true;
            rowCountCondition1.Name = "rowCountCondition1";
            rowCountCondition1.ResultSet = 1;
            rowCountCondition1.RowCount = 2;
            // 
            // GetLogs_RowCount_Test_PretestAction
            // 
            resources.ApplyResources(GetLogs_RowCount_Test_PretestAction, "GetLogs_RowCount_Test_PretestAction");
            // 
            // GetLogs_RowCount_Test_PosttestAction
            // 
            resources.ApplyResources(GetLogs_RowCount_Test_PosttestAction, "GetLogs_RowCount_Test_PosttestAction");
            // 
            // GetLogs_RowCount_TestData
            // 
            GetLogs_RowCount_TestData.PosttestAction = GetLogs_RowCount_Test_PosttestAction;
            GetLogs_RowCount_TestData.PretestAction = GetLogs_RowCount_Test_PretestAction;
            GetLogs_RowCount_TestData.TestAction = GetLogs_RowCount_Test_TestAction;
        }

        #endregion

        [TestMethod]
        public void GetLogs_RowCount_Test()
        {
            SqlDatabaseTestActions testActions = GetLogs_RowCount_TestData;

            Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(PrivilegedContext, PrivilegedContext,
                testActions.PretestAction);
            try
            {
                Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(ExecutionContext, PrivilegedContext,
                    testActions.TestAction);
            }
            finally
            {
                Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(PrivilegedContext, PrivilegedContext,
                    testActions.PosttestAction);
            }
        }
    }
}
