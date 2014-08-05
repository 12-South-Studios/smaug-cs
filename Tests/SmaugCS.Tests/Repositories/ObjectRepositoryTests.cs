using System;
using System.Data;
using System.IO;
using System.Text;
using Moq;
using Ninject;
using NUnit.Framework;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Lua;
using Realm.Library.Patterns.Repository;
using SmallDBConnectivity;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Logging;
using SmaugCS.LuaHelpers;
using SmaugCS.Managers;
using SmaugCS.Repositories;

namespace SmaugCS.Tests.Repositories
{
    [TestFixture]
    public class ObjectRepositoryTests
    {
        private static string GetObjectLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newObject = LCreateObject(800, \"pearl wand\");");
            sb.Append("object.this = newObject;");
            sb.Append("object.this.Type = \"weapon\";");
            sb.Append("object.this.ShortDescription = \"a pearl wand\";");
            sb.Append("object.this.LongDescription = \"The ground seems to cradle a pearl wand here.\";");
            sb.Append("object.this.Action = \"blast\";");
            sb.Append("object.this.Flags = \"magic antigood antievil\";");
            sb.Append("object.this.WearFlags = \"take wield\";");
            sb.Append("object.this:SetValues(12, 4, 8, 6, 0, 0);");
            sb.Append("object.this:AddAffect(-1, -1, 60, 14, 0)");
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

        [SetUp]
        public void OnSetup()
        {
            var mockKernel = new Mock<IKernel>();
            var mockDb = new Mock<ISmallDb>();
            var mockCnx = new Mock<IDbConnection>();
            var mockLogger = new Mock<ILogWrapper>();
            var mockTimer = new Mock<ITimer>();

            LuaManager luaMgr = new LuaManager(mockLogger.Object, string.Empty);
            LogManager logMgr = new LogManager(mockLogger.Object, mockKernel.Object, mockDb.Object, mockCnx.Object,
                mockTimer.Object);

            var mockLogManager = new Mock<ILogManager>();
            DatabaseManager dbMgr = new DatabaseManager(mockLogManager.Object);

            LuaObjectFunctions.InitializeReferences(luaMgr, dbMgr);
            LuaCreateFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);

            dbMgr.OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>().Clear();

            _proxy = new LuaInterfaceProxy();

            var luaFuncRepo = LuaHelper.RegisterFunctionTypes(null, typeof(LuaObjectFunctions));
            luaFuncRepo = LuaHelper.RegisterFunctionTypes(luaFuncRepo, typeof(LuaCreateFunctions));
            _proxy.RegisterFunctions(luaFuncRepo);

            luaMgr.InitializeLuaProxy(_proxy);
        }

        [Test]
        public void LuaCreateObjectTest()
        {
            var result = LuaObjectFunctions.LuaProcessObject(GetObjectLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(800));
            Assert.That(result.Name, Is.EqualTo("pearl wand"));
            Assert.That(result.ShortDescription, Is.EqualTo("a pearl wand"));
            Assert.That(result.LongDescription, Is.EqualTo("The ground seems to cradle a pearl wand here."));
            Assert.That(result.Action, Is.EqualTo("blast"));
            Assert.That(result.Flags, Is.EqualTo("magic antigood antievil"));
            Assert.That(result.WearFlags, Is.EqualTo("take wield"));
            Assert.That(result.Spells.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(result.Spells[0], Is.EqualTo("armor"));
            Assert.That(result.Weight, Is.EqualTo(1));
            Assert.That(result.Cost, Is.EqualTo(2500));
            Assert.That(result.Rent, Is.EqualTo(250));
            Assert.That(result.Level, Is.EqualTo(0));
            Assert.That(result.Layers, Is.EqualTo(0));
        }

        [Test]
        public void LuaCreateObject_Values_Test()
        {
            var result = LuaObjectFunctions.LuaProcessObject(GetObjectLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value[0], Is.EqualTo(12));
            Assert.That(result.Value[1], Is.EqualTo(4));
            Assert.That(result.Value[2], Is.EqualTo(8));
            Assert.That(result.Value[3], Is.EqualTo(6));
            Assert.That(result.Value[4], Is.EqualTo(0));
            Assert.That(result.Value[5], Is.EqualTo(0));
        }

        [Test]
        public void LuaCreateObject_Affects_Test()
        {
            var result = LuaObjectFunctions.LuaProcessObject(GetObjectLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Affects.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(result.Affects[0].Type, Is.EqualTo(AffectedByTypes.None));
            Assert.That(result.Affects[0].Duration, Is.EqualTo(-1));
            Assert.That(result.Affects[0].Modifier, Is.EqualTo(60));
            Assert.That(result.Affects[0].Location, Is.EqualTo(ApplyTypes.Hit));
            Assert.That(result.Affects[0].BitVector.IsEmpty(), Is.True);
        }

        [Test]
        public void LuaCreateObject_ExtraDescriptions_Test()
        {
            var result = LuaObjectFunctions.LuaProcessObject(GetObjectLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ExtraDescriptions.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(result.ExtraDescriptions.Find(x => x.Keyword.Equals("wand")), Is.Not.Null);
            Assert.That(result.ExtraDescriptions.Find(x => x.Keyword.Equals("pearl")), Is.Not.Null);
        }

        [Test]
        public void LuaCreateObject_MudProgs_Test()
        {
            var result = LuaObjectFunctions.LuaProcessObject(GetObjectLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.MudProgs.Count, Is.EqualTo(1));
            Assert.That(result.MudProgs[0].Type, Is.EqualTo(MudProgTypes.Damage));
            Assert.That(result.MudProgs[0].ArgList, Is.EqualTo("100"));
            Assert.That(result.MudProgs[0].Script, Is.EqualTo("local ch = GetLastCharacter();MPEcho(\"Testing\", ch);LObjectCommand(\"c fires $n\", ch);"));
        }

        [Test]
        public void Create()
        {
            var repo = new ObjectRepository();

            var actual = repo.Create(1, "Test");

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Vnum, Is.EqualTo(1));
            Assert.That(actual.Name, Is.EqualTo("Test"));
            Assert.That(actual.ShortDescription, Is.EqualTo("A newly created Test"));
            Assert.That(repo.Contains(1), Is.True);
        }

        [Test]
        public void Create_CloneObject()
        {
            var repo = new ObjectRepository();

            var source = repo.Create(1, "Test");
            source.ShortDescription = "This is a test";

            var cloned = repo.Create(2, 1, "Test2");

            Assert.That(cloned, Is.Not.Null);
            Assert.That(cloned.Vnum, Is.EqualTo(2));
            Assert.That(cloned.Name, Is.EqualTo("Test2"));
            Assert.That(cloned.ShortDescription, Is.EqualTo(source.ShortDescription));
            Assert.That(repo.Contains(2), Is.True);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_ThrowsException()
        {
            var repo = new ObjectRepository();

            repo.Create(1, "");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_ThrowsException_InvalidVnum()
        {
            var repo = new ObjectRepository();

            repo.Create(0, "Test");
        }

        [Test]
        [ExpectedException(typeof(DuplicateIndexException))]
        public void Create_DuplicateVnum()
        {
            var repo = new ObjectRepository();

            repo.Create(1, "Test");
            repo.Create(1, "Test2");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Clone_InvalidCloneVnum()
        {
            var repo = new ObjectRepository();

            repo.Create(1, 1, "Test");
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void Create_Clone_MissingCloneObject()
        {
            var repo = new ObjectRepository();

            repo.Create(1, "Test");
            repo.Create(2, 5, "Test2");
        }
    }
}
