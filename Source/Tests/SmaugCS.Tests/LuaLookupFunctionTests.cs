using FakeItEasy;
using FluentAssertions;
using SmaugCS.Data.Exceptions;
using SmaugCS.Logging;
using SmaugCS.LuaHelpers;
using SmaugCS.Managers;
using Xunit;

namespace SmaugCS.Tests;

public class LuaLookupFunctionTests
{
  private LookupManager LookupMgr { get; set; } = new();

  [Fact]
  public void LuaAddLookupTest()
  {
    ILogManager mockLogger = A.Fake<ILogManager>();

    LuaLookupFunctions.InitializeReferences(LookupMgr, mockLogger);

    LuaLookupFunctions.LuaAddLookup("TestTable", "This is a test entry");

    LookupMgr.HasLookup("TestTable", "This is a test entry").Should().BeTrue();
  }

  [Fact]
  public void LuaAddLookup_AlreadyPresent_Test()
  {
    bool callbackValue = false;

    ILogManager mockLogger = A.Fake<ILogManager>();
    A.CallTo(() => mockLogger.Boot(A<DuplicateEntryException>.Ignored)).Invokes(() => callbackValue = true);

    LuaLookupFunctions.InitializeReferences(LookupMgr, mockLogger);

    // Add once to enter it into the list
    LuaLookupFunctions.LuaAddLookup("TestTable", "This is a test entry");

    // Add it again to verify an exception is logged
    LuaLookupFunctions.LuaAddLookup("TestTable", "This is a test entry");

    callbackValue.Should().BeTrue();
  }
}