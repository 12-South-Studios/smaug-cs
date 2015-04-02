using System.Data;
using System.Linq;
using System.Text;
using Moq;
using Ninject;
using NUnit.Framework;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Lua;
using Realm.Library.Patterns.Repository;
using Realm.Library.SmallDb;
using SmaugCS.Data;
using SmaugCS.Data.Templates;
using SmaugCS.DAL.Interfaces;
using SmaugCS.Logging;
using SmaugCS.Lua;
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
            LuaManager luaMgr = new LuaManager(new Mock<IKernel>().Object, new Mock<ILogWrapper>().Object, string.Empty);

            DatabaseManager dbMgr = new DatabaseManager(new Mock<ILogManager>().Object);

            LogManager logMgr = new LogManager(new Mock<ILogWrapper>().Object, new Mock<IKernel>().Object,
                new Mock<ITimer>().Object, new Mock<ISmaugDbContext>().Object);

            LuaAreaFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);
            LuaRoomFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);

            dbMgr.AREAS.Clear();
            dbMgr.ROOMS.CastAs<Repository<long, RoomTemplate>>().Clear();

            var luaProxy = new LuaInterfaceProxy();;

            var luaFuncRepo = LuaHelper.Register( typeof(LuaAreaFunctions), null);
            luaFuncRepo = LuaHelper.Register(typeof(LuaRoomFunctions), luaFuncRepo);
            luaProxy.RegisterFunctions(luaFuncRepo);

            luaMgr.InitializeLuaProxy(luaProxy);
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
            Assert.That(result.Rooms.Count(), Is.EqualTo(1));
            Assert.That(result.Rooms.First().ID, Is.EqualTo(801));
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
