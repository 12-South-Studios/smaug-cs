using FakeItEasy;
using FluentAssertions;
using Library.Common;
using Library.Common.Logging;
using Library.Common.Objects;
using Library.Lua;
using Patterns.Repository;
using SmaugCS.DAL;
using SmaugCS.Data.Templates;
using SmaugCS.Logging;
using SmaugCS.Lua;
using SmaugCS.Repository;
using System.Linq;
using System.Text;
using SmaugCS.Data;
using SmaugCS.LuaHelpers;
using Test.Common;
using Xunit;

namespace SmaugCS.Tests.Repositories;

[Collection(CollectionDefinitions.NonParallelCollection)]
public class AreaRepositoryTests
{
  private static string GetAreaLuaScript()
  {
    StringBuilder sb = new();
    sb.Append("newArea = LCreateArea(1, \"The Astral\");");
    sb.Append("area.this = newArea;");
    sb.Append("area.this.Author = \"Andi\";");
    sb.Append("area.this.WeatherX = 0;");
    sb.Append("area.this.WeatherY = 0;");
    sb.Append("area.this.LowSoftRange = 15;");
    sb.Append("area.this.HighSoftRange = 35;");
    sb.Append("area.this.LowHardRange = 0;");
    sb.Append("area.this.HighHardRange = 60;");
    sb.Append("area.this.HighEconomy = 2064393;");
    sb.Append(
      "area.this.ResetMessage = \"The astral field glitters with a thousand points of light for a moment...\";");

    sb.Append("newRoom = LCreateRoom(801, \"On the Rainbow\");");
    sb.Append("room.this = newRoom;");
    sb.Append("area.this:AddRoom(room.this);");
    return sb.ToString();
  }

  public AreaRepositoryTests()
  {
    LuaManager luaMgr = new(A.Fake<ILogWrapper>());

    RepositoryManager dbMgr = new(A.Fake<ILogManager>());

    LogManager logMgr = new(A.Fake<ILogWrapper>(), A.Fake<ITimer>(), A.Fake<IDbContext>());

    LuaAreaFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);
    LuaRoomFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);

    dbMgr.AREAS.Clear();
    dbMgr.ROOMS.CastAs<Repository<long, RoomTemplate>>().Clear();

    LuaInterfaceProxy luaProxy = new();

    LuaFunctionRepository luaFuncRepo = LuaHelper.Register(typeof(LuaAreaFunctions), null);
    luaFuncRepo = LuaHelper.Register(typeof(LuaRoomFunctions), luaFuncRepo);
    luaProxy.RegisterFunctions(luaFuncRepo);

    luaMgr.InitializeLuaProxy(luaProxy);
  }

  [Fact]
  public void LuaCreateAreaTest()
  {
    AreaData result = LuaAreaFunctions.LuaProcessArea(GetAreaLuaScript());

    result.Should().NotBeNull();
    result.Id.Should().Be(1);
    result.Name.Should().Be("The Astral");
    result.Author.Should().Be("Andi");
    result.HighEconomy.Should().Be(2064393);
    result.HighHardRange.Should().Be(60);
    result.ResetMessage.Should().Be("The astral field glitters with a thousand points of light for a moment...");
  }

  [Fact]
  public void LuaCreateArea_AddRoom_Test()
  {
    AreaData result = LuaAreaFunctions.LuaProcessArea(GetAreaLuaScript());

    result.Should().NotBeNull();
    result.Rooms.Count().Should().Be(1);
    result.Rooms.First().Id.Should().Be(801);
  }

  /*[Fact]
  [ExpectedException(typeof(DuplicateEntryException))]
  public void LuaCreateAreaDuplicateTest()
  {
      LuaAreaFunctions.LuaProcessArea(GetAreaLuaScript();
      LuaAreaFunctions.LuaProcessArea(GetAreaLuaScript();

      Assert.Fail("Unit test expected a DuplicateEntryException to be thrown!");
  }*/
}