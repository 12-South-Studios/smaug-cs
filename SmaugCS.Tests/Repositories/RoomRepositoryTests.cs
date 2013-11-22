using System;
using System.Text;
using NUnit.Framework;
using Realm.Library.Lua;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Database;
using SmaugCS.Exceptions;
using SmaugCS.Managers;

namespace SmaugCS.Tests.Repositories
{
    [TestFixture]
    public class RoomRepositoryTests
    {
        private static string GetRoomLuaScript()
        {
            var sb = new StringBuilder();
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

            sb.Append("room.this:AddExit(exit.this);");
            sb.Append("room.this:AddExit(\"down\", 800, \"The rainbow extends below you towards Darkhaven.\");");

            sb.Append("room.this:AddReset(\"door\", 15, 807, 2, 2);");
            sb.Append("newReset = LCreateReset(\"mob\", 0, 100, 801, 1);");
            sb.Append("reset.this = newReset;");
            sb.Append("reset.this:AddReset(\"give\", 0, 110, 0, 0);");
            sb.Append("room.this:AddReset(reset.this);");

            return sb.ToString();
        }

        private LuaInterfaceProxy _proxy;

        [SetUp]
        public void OnSetup()
        {
            DatabaseManager.Instance.ROOMS.Clear();

            _proxy = new LuaInterfaceProxy();

            var luaFuncRepo = LuaHelper.RegisterFunctionTypes(null, typeof(RoomRepository));
            luaFuncRepo = LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaFunctions));
            _proxy.RegisterFunctions(luaFuncRepo);

            LuaManager.Instance.InitProxy(_proxy);
        }

        [Test]
        public void LuaCreateRoomTest()
        {
            var result = RoomRepository.LuaProcessRoom(GetRoomLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.SectorType, Is.EqualTo(SectorTypes.Air));
            Assert.That(result.Description, Is.EqualTo("This is a test description"));
        }

        [Test]
        public void LuaCreateRoom_Exits_Test()
        {
            var result = RoomRepository.LuaProcessRoom(GetRoomLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Exits.Count, Is.EqualTo(2));
            Assert.That(result.Exits[0].Direction, Is.EqualTo(DirectionTypes.Up));
            Assert.That(result.Exits[0].Destination, Is.EqualTo(802));
            Assert.That(result.Exits[0].Keywords, Is.EqualTo("gate"));
            Assert.That(result.Exits[0].Key, Is.EqualTo(800));
            Assert.That(result.Exits[1].Direction, Is.EqualTo(DirectionTypes.Down));
            Assert.That(result.Exits[1].Destination, Is.EqualTo(800));
        }

        [Test]
        public void LuaCreateRoom_ExtraDescriptions_Test()
        {
            var result = RoomRepository.LuaProcessRoom(GetRoomLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ExtraDescriptions.Count, Is.EqualTo(1));
            Assert.That(result.ExtraDescriptions.Find(x => x.Keyword.Equals("rainbow")), Is.Not.Null);
        }

        [Test]
        public void LuaCreateRoom_SetFlags_Test()
        {
            var result = RoomRepository.LuaProcessRoom(GetRoomLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Exits[0].Flags.IsSet((int)ExitFlags.IsDoor), Is.True);
            Assert.That(result.Exits[0].Flags.IsSet((int)ExitFlags.Locked), Is.True);
            Assert.That(result.Exits[0].Flags.IsSet((int)ExitFlags.Hidden), Is.False);
        }

        [Test]
        public void LuaCreateRoom_Resets_Test()
        {
            var result = RoomRepository.LuaProcessRoom(GetRoomLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Resets.Count, Is.EqualTo(2));
            Assert.That(result.Resets[0].Type, Is.EqualTo(ResetTypes.Door));
            Assert.That(result.Resets[0].Extra, Is.EqualTo(15));
            Assert.That(result.Resets[0].Args[0], Is.EqualTo(807));
            Assert.That(result.Resets[0].Args[1], Is.EqualTo(2));
            Assert.That(result.Resets[0].Args[2], Is.EqualTo(2));
        }

        [Test]
        [ExpectedException(typeof(DuplicateEntryException))]
        public void LuaCreateRoomDuplicateTest()
        {
            RoomRepository.LuaProcessRoom(GetRoomLuaScript());
            RoomRepository.LuaProcessRoom(GetRoomLuaScript());

            Assert.Fail("Unit test expected a DuplicateEntryException to be thrown!");
        }

        [Test]
        public void Create_ThrowsException()
        {
            var repo = new RoomRepository();

            var result = repo.Create(1, null);

            Assert.That(result.Name, Is.EqualTo("Floating in a Void"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_ThrowsException_InvalidVnum()
        {
            var repo = new RoomRepository();

            repo.Create(0, null);
        }

        /*[Test]
        [ExpectedException(typeof(DuplicateIndexException))]
        public void Create_DuplicateVnum_Test()
        {
            var area = new AreaData();

            var repo = new RoomRepository();

            repo.Create(1, area);
            repo.Create(1, area);
        }*/
    }
}
