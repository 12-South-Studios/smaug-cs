using System;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using Ninject;
using NUnit.Framework;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Lua;
using Realm.Library.Patterns.Repository;
using SmaugCS.Constants.Enums;
using SmaugCS.DAL.Interfaces;
using SmaugCS.Data.Templates;
using SmaugCS.Logging;
using SmaugCS.Lua;
using SmaugCS.LuaHelpers;
using SmaugCS.Repository;

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

        [SetUp]
        public void OnSetup()
        {
            var mockKernel = new Mock<IKernel>();
            var mockCtx = new Mock<ISmaugDbContext>();
            var mockLogger = new Mock<ILogWrapper>();
            var mockTimer = new Mock<ITimer>();

            LuaManager luaMgr = new LuaManager(new Mock<IKernel>().Object, mockLogger.Object);
            LogManager logMgr = new LogManager(mockLogger.Object, mockKernel.Object, mockTimer.Object, mockCtx.Object, 0);

            var mockLogManager = new Mock<ILogManager>();
            RepositoryManager dbMgr = new RepositoryManager(mockKernel.Object, mockLogManager.Object);

            LuaObjectFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);
            LuaCreateFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);

            dbMgr.OBJECTTEMPLATES.CastAs<Repository<long, ObjectTemplate>>().Clear();

            _proxy = new LuaInterfaceProxy();

            var luaFuncRepo = LuaHelper.Register(typeof(LuaObjectFunctions), null);
            luaFuncRepo = LuaHelper.Register(typeof(LuaCreateFunctions), luaFuncRepo);
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
            Assert.That(result.Spells.ToList()[0], Is.EqualTo("armor"));
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
            Assert.That(result.Values, Is.Not.Null);
            Assert.That(result.Values.Condition, Is.EqualTo(12));
            Assert.That(result.Values.NumberOfDice, Is.EqualTo(4));
            Assert.That(result.Values.SizeOfDice, Is.EqualTo(8));
            Assert.That(result.Values.WeaponType, Is.EqualTo(6));
        }

        [Test]
        public void LuaCreateObject_Affects_Test()
        {
            var result = LuaObjectFunctions.LuaProcessObject(GetObjectLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Affects.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(result.Affects.ToList()[0].Type, Is.EqualTo(AffectedByTypes.None));
            Assert.That(result.Affects.ToList()[0].Duration, Is.EqualTo(-1));
            Assert.That(result.Affects.ToList()[0].Modifier, Is.EqualTo(60));
            Assert.That(result.Affects.ToList()[0].Location, Is.EqualTo(ApplyTypes.Hit));
            Assert.That(result.Affects.ToList()[0].Flags, Is.EqualTo(32));
        }

        [Test]
        public void LuaCreateObject_ExtraDescriptions_Test()
        {
            var result = LuaObjectFunctions.LuaProcessObject(GetObjectLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ExtraDescriptions.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(result.ExtraDescriptions.ToList().Find(x => x.Keyword.Equals("wand")), Is.Not.Null);
            Assert.That(result.ExtraDescriptions.ToList().Find(x => x.Keyword.Equals("pearl")), Is.Not.Null);
        }

        [Test]
        public void LuaCreateObject_MudProgs_Test()
        {
            var result = LuaObjectFunctions.LuaProcessObject(GetObjectLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.MudProgs.Count(), Is.EqualTo(1));
            Assert.That(result.MudProgs.First().Type, Is.EqualTo(MudProgTypes.Damage));
            Assert.That(result.MudProgs.First().ArgList, Is.EqualTo("100"));
            Assert.That(result.MudProgs.First().Script,
                Is.EqualTo("local ch = GetLastCharacter();MPEcho(\"Testing\", ch);LObjectCommand(\"c fires $n\", ch);"));
        }

        [Test]
        public void Create()
        {
            var repo = new ObjectRepository();

            var actual = repo.Create(1, "Test");

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.ID, Is.EqualTo(1));
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
            Assert.That(cloned.ID, Is.EqualTo(2));
            Assert.That(cloned.Name, Is.EqualTo("Test2"));
            Assert.That(cloned.ShortDescription, Is.EqualTo(source.ShortDescription));
            Assert.That(repo.Contains(2), Is.True);
        }

        [Test]
        public void Create_ThrowsException()
        {
            var repo = new ObjectRepository();

            Assert.Throws<ArgumentException>(() => repo.Create(1, ""));
        }

        [Test]
        public void Create_ThrowsException_InvalidVnum()
        {
            var repo = new ObjectRepository();

            Assert.Throws<ArgumentException>(() => repo.Create(0, "Test"));
        }

        [Test]
        public void Create_DuplicateVnum()
        {
            var repo = new ObjectRepository();

            repo.Create(1, "Test");
            Assert.Throws<DuplicateIndexException>(() => repo.Create(1, "Test2"));
        }

        [Test]
        public void Create_Clone_InvalidCloneVnum()
        {
            var repo = new ObjectRepository();

            Assert.Throws<ArgumentException>(() => repo.Create(1, 1, "Test"));
        }

        [Test]
        public void Create_Clone_MissingCloneObject()
        {
            var repo = new ObjectRepository();

            repo.Create(1, "Test");
            Assert.Throws<InvalidDataException>(() => repo.Create(2, 5, "Test2"));
        }
    }
}
