using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SmaugCS.Database.Tests
{
    [TestClass()]
    public class GetLogsTests : SqlDatabaseTestClass
    {

        public GetLogsTests()
        {
            InitializeComponent();
        }

        [TestInitialize()]
        public void TestInitialize()
        {
            base.InitializeTest();
        }
        [TestCleanup()]
        public void TestCleanup()
        {
            base.CleanupTest();
        }

        #region Designer support code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction GetLogs_RowCount_Test_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetLogsTests));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction GetLogs_RowCount_Test_PretestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction GetLogs_RowCount_Test_PosttestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition rowCountCondition1;
            this.GetLogs_RowCount_TestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            GetLogs_RowCount_Test_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            GetLogs_RowCount_Test_PretestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            GetLogs_RowCount_Test_PosttestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            rowCountCondition1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            // 
            // GetLogs_RowCount_Test_TestAction
            // 
            GetLogs_RowCount_Test_TestAction.Conditions.Add(rowCountCondition1);
            resources.ApplyResources(GetLogs_RowCount_Test_TestAction, "GetLogs_RowCount_Test_TestAction");
            // 
            // GetLogs_RowCount_TestData
            // 
            this.GetLogs_RowCount_TestData.PosttestAction = GetLogs_RowCount_Test_PosttestAction;
            this.GetLogs_RowCount_TestData.PretestAction = GetLogs_RowCount_Test_PretestAction;
            this.GetLogs_RowCount_TestData.TestAction = GetLogs_RowCount_Test_TestAction;
            // 
            // GetLogs_RowCount_Test_PretestAction
            // 
            resources.ApplyResources(GetLogs_RowCount_Test_PretestAction, "GetLogs_RowCount_Test_PretestAction");
            // 
            // GetLogs_RowCount_Test_PosttestAction
            // 
            resources.ApplyResources(GetLogs_RowCount_Test_PosttestAction, "GetLogs_RowCount_Test_PosttestAction");
            // 
            // rowCountCondition1
            // 
            rowCountCondition1.Enabled = true;
            rowCountCondition1.Name = "rowCountCondition1";
            rowCountCondition1.ResultSet = 1;
            rowCountCondition1.RowCount = 2;
        }

        #endregion


        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        #endregion


        [TestMethod()]
        public void GetLogs_RowCount_Test()
        {
            SqlDatabaseTestActions testActions = this.GetLogs_RowCount_TestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }
        private SqlDatabaseTestActions GetLogs_RowCount_TestData;
    }
}
