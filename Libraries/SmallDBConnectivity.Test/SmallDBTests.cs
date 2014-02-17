using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Moq;
using NUnit.Framework;
using SmallDBConnectivity.Test.Fakes;

namespace SmallDBConnectivity.Test
{
    [TestFixture]
    public class SmallDbTests
    {
        public class FakeObject
        {
            public string Name { get { return "Fake"; } }
        }  

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateArguments_TakesNullConnection_ThrowsException()
        {
            SmallDb.ValidateArguments(null, "TestProcedure");

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown!");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ValidateArguments_TakesEmptyStoredProcedureName_ThrowsException()
        {
            var mockConnection = new Mock<IDbConnection>();

            SmallDb.ValidateArguments(mockConnection.Object, string.Empty);

            Assert.Fail("Unit test expected an ArgumentNullException to be thrown!");
        }

        [Test]
        public void ValidateArguments_TakesValidArguments_DoesNothing()
        {
            var mockConnection = new Mock<IDbConnection>();

            try
            {
                SmallDb.ValidateArguments(mockConnection.Object, "TestProcedure");
            }
            catch (Exception ex)
            {
                Assert.Fail("ValidateArguments threw an Exception of Type {0}", ex.Message);
            }
        }

        [Test]
        public void SetupDbCommand_TakesParameters_PopulatesCommand()
        {
            var parameterList = new List<IDataParameter>
                {
                    new SqlParameter("TestParam1", SqlDbType.Int),
                    new SqlParameter("TestParam2", SqlDbType.VarChar)
                };

            var fakeCommand = new FakeDbCommand();

            var mockConnection = new Mock<IDbConnection>();

            SmallDb.SetupDbCommand(mockConnection.Object, fakeCommand, "TestProcedure", parameterList);

            Assert.That(fakeCommand.Connection, Is.EqualTo(mockConnection.Object));
            Assert.That(fakeCommand.CommandText, Is.EqualTo("TestProcedure"));
            Assert.That(fakeCommand.Parameters.Count == 2, Is.True);
        }

        [Test]
        public void ExecuteScalar_TakesNoParameters_ReturnsValidResult()
        {
            const string expected = "This is a test result";

            var mockCommand = new Mock<IDbCommand>();
            mockCommand.Setup(x => x.ExecuteScalar()).Returns(expected);
            mockCommand.SetupSet(x => x.Connection = It.IsAny<IDbConnection>());

            var mockConnection = new Mock<IDbConnection>();
            mockConnection.Setup(x => x.CreateCommand()).Returns(mockCommand.Object);

            mockCommand.SetupGet(x => x.Connection).Returns(mockConnection.Object);

            var helper = new SmallDb();
            var actualResult = helper.ExecuteScalar(mockConnection.Object, "TestProcedure");

            Assert.That(expected, Is.EqualTo(actualResult));
        }

        [Test]
        public void ExecuteNonQuery_TakesNoParameters_Executes()
        {
            var mockCommand = new Mock<IDbCommand>();
            mockCommand.SetupSet(x => x.Connection = It.IsAny<IDbConnection>());

            var mockConnection = new Mock<IDbConnection>();
            mockConnection.Setup(x => x.CreateCommand()).Returns(mockCommand.Object);

            mockCommand.SetupGet(x => x.Connection).Returns(mockConnection.Object);

            var helper = new SmallDb();
            try
            {
                helper.ExecuteNonQuery(mockConnection.Object, "TestProcedure");
            }
            catch (Exception ex)
            {
                Assert.Fail("ExecuteNonQuery threw an Exception of type {0}", ex.Message);
            }
        }

        [Test]
        public void ExecuteQuery_TakesNoParameters_ReturnsValidDataTable()
        {
            var mockReader = new Mock<IDataReader>();

            var mockCommand = new Mock<IDbCommand>();
            mockCommand.SetupSet(x => x.Connection = It.IsAny<IDbConnection>());
            mockCommand.Setup(x => x.ExecuteReader()).Returns(mockReader.Object);

            var mockConnection = new Mock<IDbConnection>();
            mockConnection.Setup(x => x.CreateCommand()).Returns(mockCommand.Object);

            mockCommand.SetupGet(x => x.Connection).Returns(mockConnection.Object);

            var helper = new SmallDb();
            var result = helper.ExecuteQuery(mockConnection.Object, "TestProcedure");

            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Helper function to pass into ExecuteQuery as a delegate
        /// </summary>
        private static FakeObject CreateFakeObject(IDataReader reader)
        {
            return new FakeObject();
        }

        [Test]
        public void ExecuteQueryWithFunc_TakesNoParameters_ReturnsValidObject()
        {
            var mockReader = new Mock<IDataReader>();

            var mockCommand = new Mock<IDbCommand>();
            mockCommand.SetupSet(x => x.Connection = It.IsAny<IDbConnection>());
            mockCommand.Setup(x => x.ExecuteReader()).Returns(mockReader.Object);

            var mockConnection = new Mock<IDbConnection>();
            mockConnection.Setup(x => x.CreateCommand()).Returns(mockCommand.Object);

            mockCommand.SetupGet(x => x.Connection).Returns(mockConnection.Object);

            var helper = new SmallDb();

            var result = helper.ExecuteQuery(mockConnection.Object, "TestProcedure", CreateFakeObject);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<FakeObject>());
            Assert.That(result.Name, Is.EqualTo("Fake"));
        }
   }
}
