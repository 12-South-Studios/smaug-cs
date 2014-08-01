using Moq;
using NUnit.Framework;
using SmaugCS.Logging;
using SmaugCS.LuaHelpers;
using SmaugCS.Managers;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class LuaLookupFunctionTests
    {
        public LookupManager LookupMgr { get; set; }

        [SetUp]
        public void OnSetup()
        {
            LookupMgr = new LookupManager();
        }

        [Test]
        public void LuaAddLookupTest()
        {
            var mockLogger = new Mock<ILogManager>();

            LuaLookupFunctions.InitializeReferences(LookupMgr, mockLogger.Object);

            LuaLookupFunctions.LuaAddLookup("TestTable", "This is a test entry");

            Assert.That(LookupMgr.HasLookup("TestTable", "This is a test entry"), Is.True);
        }

        [Test]
        public void LuaAddLookup_AlreadyPresent_Test()
        {
            var callbackValue = false;

            var mockLogger = new Mock<ILogManager>();
            mockLogger.Setup(x => x.Boot(It.IsAny<DuplicateEntryException>())).Callback(() => callbackValue = true);

            LuaLookupFunctions.InitializeReferences(LookupMgr, mockLogger.Object);

            // Add once to enter it into the list
            LuaLookupFunctions.LuaAddLookup("TestTable", "This is a test entry");

            // Add it again to verify an exception is logged
            LuaLookupFunctions.LuaAddLookup("TestTable", "This is a test entry"); 

            Assert.That(callbackValue, Is.True);
        }
    }
}
