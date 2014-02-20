using System;
using Moq;
using NUnit.Framework;
using SmaugCS.Data.Interfaces;
using SmaugCS.Exceptions;
using SmaugCS.Logging;
using SmaugCS.LuaHelpers;
using SmaugCS.Managers;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class LuaLookupFunctionTests
    {
        [Test]
        public void LuaAddLookupTest()
        {
            var mockLogger = new Mock<ILogManager>();

            var lookupManager = LookupManager.Instance;
            lookupManager.Initialize();

            LuaLookupFunctions.InitializeReferences(lookupManager, mockLogger.Object);

            LuaLookupFunctions.LuaAddLookup("TestTable", "This is a test entry");

            Assert.That(LookupManager.Instance.HasLookup("TestTable", "This is a test entry"), Is.True);
        }

        [Test]
        public void LuaAddLookup_AlreadyPresent_Test()
        {
            var callbackValue = false;

            var mockLogger = new Mock<ILogManager>();
            mockLogger.Setup(x => x.BootLog(It.IsAny<DuplicateEntryException>())).Callback(() => callbackValue = true);

            var lookupManager = LookupManager.Instance;
            lookupManager.Initialize();

            LuaLookupFunctions.InitializeReferences(lookupManager, mockLogger.Object);

            // Add once to enter it into the list
            LuaLookupFunctions.LuaAddLookup("TestTable", "This is a test entry");

            // Add it again to verify an exception is logged
            LuaLookupFunctions.LuaAddLookup("TestTable", "This is a test entry"); 

            Assert.That(callbackValue, Is.True);
        }
    }
}
