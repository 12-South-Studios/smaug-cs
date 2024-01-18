using FakeItEasy;
using FluentAssertions;
using Ninject;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Common.Objects;
using Realm.Library.Lua;
using Realm.Standard.Patterns.Repository;
using SmaugCS.DAL;
using SmaugCS.Data.Templates;
using SmaugCS.Logging;
using SmaugCS.Lua;
using SmaugCS.Repository;
using System.Linq;
using System.Text;
using Test.Common;
using Xunit;

namespace SmaugCS.Tests.Repositories
{
    [Collection(CollectionDefinitions.NonParallelCollection)]
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

        public AreaRepositoryTests()
        {
            LuaManager luaMgr = new LuaManager(A.Fake<IKernel>(), A.Fake<ILogWrapper>());

            RepositoryManager dbMgr = new RepositoryManager(A.Fake<IKernel>(), A.Fake<ILogManager>());

            LogManager logMgr = new LogManager(A.Fake<ILogWrapper>(), A.Fake<IKernel>(),
                A.Fake<ITimer>(), A.Fake<IDbContext>(), 0);

            LuaAreaFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);
            LuaRoomFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);

            dbMgr.AREAS.Clear();
            dbMgr.ROOMS.CastAs<Repository<long, RoomTemplate>>().Clear();

            var luaProxy = new LuaInterfaceProxy();

            var luaFuncRepo = LuaHelper.Register(typeof(LuaAreaFunctions), null);
            luaFuncRepo = LuaHelper.Register(typeof(LuaRoomFunctions), luaFuncRepo);
            luaProxy.RegisterFunctions(luaFuncRepo);

            luaMgr.InitializeLuaProxy(luaProxy);
        }

        [Fact]
        public void LuaCreateAreaTest()
        {
            var result = LuaAreaFunctions.LuaProcessArea(GetAreaLuaScript());

            result.Should().NotBeNull();
            result.ID.Should().Be(1);
            result.Name.Should().Be("The Astral");
            result.Author.Should().Be("Andi");
            result.HighEconomy.Should().Be(2064393);
            result.HighHardRange.Should().Be(60);
            result.ResetMessage.Should().Be("The astral field glitters with a thousand points of light for a moment...");
        }

        [Fact]
        public void LuaCreateArea_AddRoom_Test()
        {
            var result = LuaAreaFunctions.LuaProcessArea(GetAreaLuaScript());

            result.Should().NotBeNull();
            result.Rooms.Count().Should().Be(1);
            result.Rooms.First().ID.Should().Be(801);
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
}
