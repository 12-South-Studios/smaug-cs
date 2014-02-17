﻿using System.Text;
using NUnit.Framework;
using Realm.Library.Common;
using Realm.Library.Lua;
using Realm.Library.Patterns.Repository;
using SmaugCS.Data.Templates;
using SmaugCS.Database;
using SmaugCS.Exceptions;
using SmaugCS.LuaHelpers;
using SmaugCS.Managers;

namespace SmaugCS.Tests.Repositories
{
    [TestFixture]
    public class AreaRepositoryTests
    {
        private static string GetAreaLuaScript()
        {
            var sb = new StringBuilder();
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

        [SetUp]
        public void OnSetup()
        {
            LuaAreaFunctions.InitializeReferences(LuaManager.Instance, DatabaseManager.Instance);
            LuaRoomFunctions.InitializeReferences(LuaManager.Instance, DatabaseManager.Instance);

            DatabaseManager.Instance.AREAS.Clear();
            DatabaseManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Clear();

            var luaProxy = new LuaInterfaceProxy();

            var luaFuncRepo = LuaHelper.RegisterFunctionTypes(null, typeof(LuaAreaFunctions));
            luaFuncRepo = LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaRoomFunctions));
            luaProxy.RegisterFunctions(luaFuncRepo);

            LuaManager.Instance.InitializeLuaProxy(luaProxy);
        }

        [Test]
        public void LuaCreateAreaTest()
        {
            var result = LuaAreaFunctions.LuaProcessArea(GetAreaLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("The Astral"));
            Assert.That(result.Author, Is.EqualTo("Andi"));
            Assert.That(result.HighEconomy, Is.EqualTo(2064393));
            Assert.That(result.HighHardRange, Is.EqualTo(60));
            Assert.That(result.ResetMessage, Is.EqualTo("The astral field glitters with a thousand points of light for a moment..."));
        }

        [Test]
        public void LuaCreateArea_AddRoom_Test()
        {
            var result = LuaAreaFunctions.LuaProcessArea(GetAreaLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Rooms.Count, Is.EqualTo(1));
            Assert.That(result.Rooms[0].ID, Is.EqualTo(801));
        }

        /*[Test]
        [ExpectedException(typeof(DuplicateEntryException))]
        public void LuaCreateAreaDuplicateTest()
        {
            LuaAreaFunctions.LuaProcessArea(GetAreaLuaScript());
            LuaAreaFunctions.LuaProcessArea(GetAreaLuaScript());

            Assert.Fail("Unit test expected a DuplicateEntryException to be thrown!");
        }*/
    }
}