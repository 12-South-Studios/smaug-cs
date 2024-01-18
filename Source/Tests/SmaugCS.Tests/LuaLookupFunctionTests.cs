using FakeItEasy;
using FluentAssertions;
using SmaugCS.Data.Exceptions;
using SmaugCS.Logging;
using Xunit;

namespace SmaugCS.Tests
{

    public class LuaLookupFunctionTests
    {
        private LookupManager LookupMgr { get; set; }

        public LuaLookupFunctionTests()
        {
            LookupMgr = new LookupManager();
        }

        [Fact]
        public void LuaAddLookupTest()
        {
            var mockLogger = A.Fake<ILogManager>();

            LuaLookupFunctions.InitializeReferences(LookupMgr, mockLogger);

            LuaLookupFunctions.LuaAddLookup("TestTable", "This is a test entry");

            LookupMgr.HasLookup("TestTable", "This is a test entry").Should().BeTrue();
        }

        [Fact]
        public void LuaAddLookup_AlreadyPresent_Test()
        {
            var callbackValue = false;

            var mockLogger = A.Fake<ILogManager>();
            A.CallTo(() => mockLogger.Boot(A<DuplicateEntryException>.Ignored)).Invokes(() => callbackValue = true);

            LuaLookupFunctions.InitializeReferences(LookupMgr, mockLogger);

            // Add once to enter it into the list
            LuaLookupFunctions.LuaAddLookup("TestTable", "This is a test entry");

            // Add it again to verify an exception is logged
            LuaLookupFunctions.LuaAddLookup("TestTable", "This is a test entry");

            callbackValue.Should().BeTrue();
        }
    }
}
