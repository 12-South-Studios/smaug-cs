using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using Ninject;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Common.Objects;
using Realm.Library.Lua;
using Realm.Standard.Patterns.Repository;
using SmaugCS.Constants.Enums;
using SmaugCS.DAL;
using SmaugCS.Data.Templates;
using SmaugCS.Logging;
using SmaugCS.Lua;
using SmaugCS.Repository;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Test.Common;
using Xunit;

namespace SmaugCS.Tests.Repositories
{
    [Collection(CollectionDefinitions.NonParallelCollection)]
    public class ObjectRepositoryTests
    {
        private static string GetObjectLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newObject = LCreateObject(800, \"pearl wand\");");
            sb.Append("object.this = newObject;");
            sb.Append("object.this:SetType(\"weapon\");");
            sb.Append("object.this.ShortDescription = \"a pearl wand\";");
            sb.Append("object.this.LongDescription = \"The ground seems to cradle a pearl wand here.\";");
            sb.Append("object.this.Action = \"blast\";");
            sb.Append("object.this.Flags = \"magic antigood antievil\";");
            sb.Append("object.this.WearFlags = \"take wield\";");
            sb.Append("object.this:SetValues(12, 4, 8, 6, 0, 0);");
            sb.Append("object.this:AddAffect(0, -1, 60, 14, 32)");
            sb.Append("object.this:AddSpell(\"armor\");");
            sb.Append("object.this:SetStats(1, 2500, 250, 0, 0);");
            sb.Append(
                "object.this:AddExtraDescription(\"wand pearl\", \"An intricate number of nooks have been engraved in the end of the wand, as though it was a key of some sort...\");");

            sb.Append("newProg = LCreateMudProg(\"damage_prog\");");
            sb.Append("mprog.this = newProg;");
            sb.Append("mprog.this.ArgList = \"100\";");
            sb.Append("mprog.this.Script = [[local ch = GetLastCharacter();MPEcho(\"Testing\", ch);LObjectCommand(\"c fires $n\", ch);]];");
            sb.Append("object.this:AddMudProg(mprog.this);");

            return sb.ToString();
        }

        private LuaInterfaceProxy _proxy;

        public ObjectRepositoryTests()
        {
            var mockKernel = A.Fake<IKernel>();
            var mockCtx = A.Fake<IDbContext>();
            var mockLogger = A.Fake<ILogWrapper>();
            var mockTimer = A.Fake<ITimer>();

            LuaManager luaMgr = new LuaManager(A.Fake<IKernel>(), mockLogger);
            LogManager logMgr = new LogManager(mockLogger, mockKernel, mockTimer, mockCtx, 0);

            var mockLogManager = A.Fake<ILogManager>();
            RepositoryManager dbMgr = new RepositoryManager(mockKernel, mockLogManager);

            LuaObjectFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);
            LuaCreateFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);

            dbMgr.OBJECTTEMPLATES.CastAs<Repository<long, ObjectTemplate>>().Clear();

            _proxy = new LuaInterfaceProxy();

            var luaFuncRepo = LuaHelper.Register(typeof(LuaObjectFunctions), null);
            luaFuncRepo = LuaHelper.Register(typeof(LuaCreateFunctions), luaFuncRepo);
            _proxy.RegisterFunctions(luaFuncRepo);

            luaMgr.InitializeLuaProxy(_proxy);
        }

        [Fact]
        public void LuaCreateObjectTest()
        {
            var result = LuaObjectFunctions.LuaProcessObject(GetObjectLuaScript());

            result.Should().NotBeNull();
            result.ID.Should().Be(800);
            result.Name.Should().Be("pearl wand");
            result.ShortDescription.Should().Be("a pearl wand");
            result.LongDescription.Should().Be("The ground seems to cradle a pearl wand here.");
            result.Action.Should().Be("blast");
            result.Flags.Should().Be("magic antigood antievil");
            result.WearFlags.Should().Be("take wield");
            result.Spells.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Spells.ToList()[0].Should().Be("armor");
            result.Weight.Should().Be(1);
            result.Cost.Should().Be(2500);
            result.Rent.Should().Be(250);
            result.Level.Should().Be(0);
            result.Layers.Should().Be(0);
        }

        [Fact]
        public void LuaCreateObject_Values_Test()
        {
            var result = LuaObjectFunctions.LuaProcessObject(GetObjectLuaScript());

            result.Should().NotBeNull();
            Assert.NotEqual(result.Values, null);
            Assert.Equal(12, result.Values.Condition);
            Assert.Equal(4, result.Values.NumberOfDice);
            Assert.Equal(8, result.Values.SizeOfDice);
            Assert.Equal(6, result.Values.WeaponType);
        }

        [Fact]
        public void LuaCreateObject_Affects_Test()
        {
            var result = LuaObjectFunctions.LuaProcessObject(GetObjectLuaScript());

            result.Should().NotBeNull();
            result.Affects.Count.Should().BeGreaterThanOrEqualTo(1);
            result.Affects.ToList()[0].Type.Should().Be(AffectedByTypes.None);
            result.Affects.ToList()[0].Duration.Should().Be(-1);
            result.Affects.ToList()[0].Modifier.Should().Be(60);
            result.Affects.ToList()[0].Location.Should().Be(ApplyTypes.Hit);
            result.Affects.ToList()[0].BitVector.IsSet(32).Should().BeTrue();
        }

        [Fact]
        public void LuaCreateObject_ExtraDescriptions_Test()
        {
            var result = LuaObjectFunctions.LuaProcessObject(GetObjectLuaScript());

            result.Should().NotBeNull();
            result.ExtraDescriptions.Count.Should().BeGreaterThanOrEqualTo(1);
            result.ExtraDescriptions.ToList().Find(x => x.Keyword.Equals("wand")).Should().NotBeNull();
            result.ExtraDescriptions.ToList().Find(x => x.Keyword.Equals("pearl")).Should().NotBeNull();
        }

        [Fact]
        public void LuaCreateObject_MudProgs_Test()
        {
            var result = LuaObjectFunctions.LuaProcessObject(GetObjectLuaScript());

            result.Should().NotBeNull();
            result.MudProgs.Count().Should().Be(1);
            result.MudProgs.First().Type.Should().Be(MudProgTypes.Damage);
            result.MudProgs.First().ArgList.Should().Be("100");
            result.MudProgs.First().Script.Should().Be("local ch = GetLastCharacter();MPEcho(\"Testing\", ch);LObjectCommand(\"c fires $n\", ch);");
        }

        [Fact]
        public void Create()
        {
            var repo = new ObjectRepository();

            var actual = repo.Create(1, "Test");

            actual.Should().NotBeNull();
            actual.ID.Should().Be(1);
            actual.Name.Should().Be("Test");
            actual.ShortDescription.Should().Be("A newly created Test");
            repo.Contains(1).Should().BeTrue();
        }

        [Fact]
        public void Create_CloneObject()
        {
            var repo = new ObjectRepository();

            var source = repo.Create(1, "Test");
            source.ShortDescription = "This is a test";

            var cloned = repo.Create(2, 1, "Test2");

            cloned.Should().NotBeNull();
            cloned.ID.Should().Be(2);
            cloned.Name.Should().Be("Test2");
            cloned.ShortDescription.Should().Be(source.ShortDescription);
            repo.Contains(2).Should().BeTrue();
        }

        [Fact]
        public void Create_ThrowsException()
        {
            var repo = new ObjectRepository();

            Assert.Throws<ArgumentException>(() => repo.Create(1, ""));
        }

        [Fact]
        public void Create_ThrowsException_InvalidVnum()
        {
            var repo = new ObjectRepository();

            Assert.Throws<ArgumentException>(() => repo.Create(0, "Test"));
        }

        [Fact]
        public void Create_DuplicateVnum()
        {
            var repo = new ObjectRepository();

            repo.Create(1, "Test");
            Assert.Throws<DuplicateIndexException>(() => repo.Create(1, "Test2"));
        }

        [Fact]
        public void Create_Clone_InvalidCloneVnum()
        {
            var repo = new ObjectRepository();

            Assert.Throws<ArgumentException>(() => repo.Create(1, 1, "Test"));
        }

        [Fact]
        public void Create_Clone_MissingCloneObject()
        {
            var repo = new ObjectRepository();

            repo.Create(1, "Test");
            Assert.Throws<InvalidDataException>(() => repo.Create(2, 5, "Test2"));
        }
    }
}
