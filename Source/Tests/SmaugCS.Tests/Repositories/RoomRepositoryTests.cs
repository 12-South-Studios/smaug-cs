using FakeItEasy;
using FluentAssertions;
using Library.Common;
using Library.Common.Logging;
using Library.Common.Objects;
using Library.Lua;
using Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.DAL;
using SmaugCS.Data.Templates;
using SmaugCS.Logging;
using SmaugCS.Lua;
using SmaugCS.Repository;
using System;
using System.Linq;
using System.Text;
using SmaugCS.LuaHelpers;
using Test.Common;
using Xunit;

namespace SmaugCS.Tests.Repositories;

[Collection(CollectionDefinitions.NonParallelCollection)]
public class RoomRepositoryTests
{
  private static string GetRoomLuaScript()
  {
    StringBuilder sb = new();
    sb.Append("newRoom = LCreateRoom(801, \"On the Rainbow\");");
    sb.Append("room.this = newRoom;");
    sb.Append("room.this:SetSector(\"air\");");
    sb.Append("room.this.Description = [[This is a test description]]");
    sb.Append(
      "newExit = LCreateExit(\"up\", 802, \"The rainbow extends far above you, slowly fading into darkness.\");");
    sb.Append("exit.this = newExit;");
    sb.Append("exit.this.Key = 800;");
    sb.Append("exit.this.Keywords = \"gate\"");
    sb.Append("exit.this:SetFlags(\"isdoor closed locked pickproof nopassdoor\");");
    sb.Append(
      "room.this:AddExtraDescription(\"rainbow\", \"The rainbow softly glows with beams of light in colors that defy description.\");");

    sb.Append("room.this:AddExitObject(exit.this);");
    sb.Append("room.this:AddExit(\"down\", 800, \"The rainbow extends below you towards Darkhaven.\");");

    sb.Append("room.this:AddReset(\"door\", 15, 807, 2, 2);");
    sb.Append("newReset = LCreateReset(\"mob\", 0, 100, 801, 1);");
    sb.Append("reset.this = newReset;");
    sb.Append("reset.this:AddReset(\"give\", 0, 110, 0, 0);");
    sb.Append("room.this:AddReset(reset.this);");

    return sb.ToString();
  }

  public RoomRepositoryTests()
  {
    IDbContext mockCtx = A.Fake<IDbContext>();
    ILogWrapper mockLogger = A.Fake<ILogWrapper>();
    ITimer mockTimer = A.Fake<ITimer>();

    LuaManager luaMgr = new(mockLogger);
    LogManager logMgr = new(mockLogger, mockTimer, mockCtx);

    ILogManager mockLogMgr = A.Fake<ILogManager>();
    RepositoryManager dbMgr = new(mockLogMgr);

    LuaRoomFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);
    LuaCreateFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);

    dbMgr.ROOMS.CastAs<Repository<long, RoomTemplate>>().Clear();

    LuaInterfaceProxy proxy = new();

    LuaFunctionRepository luaFuncRepo = LuaHelper.Register(typeof(LuaRoomFunctions), null);
    luaFuncRepo = LuaHelper.Register(typeof(LuaCreateFunctions), luaFuncRepo);
    proxy.RegisterFunctions(luaFuncRepo);

    luaMgr.InitializeLuaProxy(proxy);
  }

  [Fact]
  public void LuaCreateRoomTest()
  {
    RoomTemplate result = LuaRoomFunctions.LuaProcessRoom(GetRoomLuaScript());

    result.Should().NotBeNull();
    result.SectorType.Should().Be(SectorTypes.Air);
    result.Description.Should().Be("This is a test description");
  }

  [Fact]
  public void LuaCreateRoom_Exits_Test()
  {
    RoomTemplate result = LuaRoomFunctions.LuaProcessRoom(GetRoomLuaScript());

    result.Should().NotBeNull();
    result.Exits.Count.Should().Be(2);
    result.Exits.ToList()[0].Direction.Should().Be(DirectionTypes.Up);
    result.Exits.ToList()[0].Destination.Should().Be(802);
    result.Exits.ToList()[0].Keywords.Should().Be("gate");
    result.Exits.ToList()[0].Key.Should().Be(800);
    result.Exits.ToList()[1].Direction.Should().Be(DirectionTypes.Down);
    result.Exits.ToList()[1].Destination.Should().Be(800);
  }

  [Fact]
  public void LuaCreateRoom_ExtraDescriptions_Test()
  {
    RoomTemplate result = LuaRoomFunctions.LuaProcessRoom(GetRoomLuaScript());

    result.Should().NotBeNull();
    result.ExtraDescriptions.Count.Should().Be(1);
    result.ExtraDescriptions.ToList().Find(x => x.Keyword.Equals("rainbow")).Should().NotBeNull();
  }

  [Fact]
  public void LuaCreateRoom_SetFlags_Test()
  {
    RoomTemplate result = LuaRoomFunctions.LuaProcessRoom(GetRoomLuaScript());

    result.Should().NotBeNull();
    result.Exits.ToList()[0].Flags.IsSet(ExitFlags.IsDoor).Should().BeTrue();
    result.Exits.ToList()[0].Flags.IsSet(ExitFlags.Locked).Should().BeTrue();
    result.Exits.ToList()[0].Flags.IsSet(ExitFlags.Hidden).Should().BeFalse();
  }

  [Fact]
  public void LuaCreateRoom_Resets_Test()
  {
    RoomTemplate result = LuaRoomFunctions.LuaProcessRoom(GetRoomLuaScript());

    result.Should().NotBeNull();
    result.Resets.Count.Should().Be(2);
    result.Resets.ToList()[0].Type.Should().Be(ResetTypes.Door);
    result.Resets.ToList()[0].Extra.Should().Be(15);
    result.Resets.ToList()[0].Args.ToList()[0].Should().Be(807);
    result.Resets.ToList()[0].Args.ToList()[1].Should().Be(2);
    result.Resets.ToList()[0].Args.ToList()[2].Should().Be(2);
  }

  /*[Fact]
  [ExpectedException(typeof(DuplicateEntryException))]
  public void LuaCreateRoomDuplicateTest()
  {
      LuaRoomFunctions.LuaProcessRoom(GetRoomLuaScript());
      LuaRoomFunctions.LuaProcessRoom(GetRoomLuaScript());

      Assert.Fail("Unit test expected a DuplicateEntryException to be thrown!");
  }*/

  [Fact]
  public void Create_ThrowsException()
  {
    RoomRepository repo = new();

    RoomTemplate result = repo.Create(1, null);

    result.Name.Should().Be("Floating in a Void");
  }

  [Fact]
  public void Create_ThrowsException_InvalidVnum()
  {
    RoomRepository repo = new();

    Assert.Throws<ArgumentException>(() => repo.Create(0, null));
  }

  /*[Fact]
  [ExpectedException(typeof(DuplicateIndexException))]
  public void Create_DuplicateVnum_Test()
  {
      var area = new AreaData();

      var repo = new RoomRepository();

      repo.Create(1, area);
      repo.Create(1, area);
  }*/
}