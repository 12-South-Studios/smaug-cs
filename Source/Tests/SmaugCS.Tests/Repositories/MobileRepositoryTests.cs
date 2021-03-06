﻿using Moq;
using Ninject;
using NUnit.Framework;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Common.Objects;
using Realm.Library.Lua;
using Realm.Standard.Patterns.Repository;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.DAL.Interfaces;
using SmaugCS.Data.Extensions;
using SmaugCS.Data.Shops;
using SmaugCS.Data.Templates;
using SmaugCS.Logging;
using SmaugCS.Lua;
using SmaugCS.Repository;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SmaugCS.Tests.Repositories
{
    [TestFixture]
    public class MobileRepositoryTests
    {
        private static string GetMobLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newMobile = LCreateMobile(801, \"nightmare\");");
            sb.Append("mobile.this = newMobile;");
            sb.Append("mobile.this.ShortDescription = \"A pitch-black nightmare\";");
            sb.Append("mobile.this.LongDescription = \"A nightmare is here, kicking at you with its flaming hooves.\";");
            sb.Append(
                "mobile.this.Description = [[The nightmare is a wholly evil being, sent out by the rulers of the lower planes to torment mortals.  It vaguely resembles a horse, with a hide blacker than the darkest night, and hooves that burn with unholy fires.]]");
            sb.Append("mobile.this.Race = \"magical\"");
            sb.Append("mobile.this.Class = \"warrior\"");
            sb.Append("mobile.this.Position = \"standing\"");
            sb.Append("mobile.this.DefensivePosition = \"standing\"");
            sb.Append("mobile.this:SetStatistic(\"gender\", \"neuter\");");
            sb.Append("mobile.this.SpecFun = \"DoSpecCastMage\"");
            sb.Append("mobile.this:SetStatistic(\"actflags\", \"npc stayarea mountable\");");
            sb.Append(
                "mobile.this:SetStatistic(\"affectedbyflags\", \"detect_evil detect_magic\");");
            sb.Append("mobile.this:SetStats1(-950, 18, 2, -2, 6000, 32000);");
            sb.Append("mobile.this:SetStats2(18, 18, 180);");
            sb.Append("mobile.this:SetStats3(5, 3, 10);");
            sb.Append("mobile.this:SetStats4(50, 100, 2, 5, 6);");
            sb.Append("mobile.this:SetAttributes(11, 12, 13, 14, 15, 16, 17);");
            sb.Append("mobile.this:SetSaves(3, 5, 3, 5, 3);");
            sb.Append("mobile.this.Speaks = \"magical\"");
            sb.Append("mobile.this.Speaking = \"magical\"");
            sb.Append("mobile.this.BodyParts = \"head legs heart guts feet\"");
            sb.Append("mobile.this.Resistance = \"sleep charm hold\"");
            sb.Append("mobile.this.Susceptibility = \"fire blunt\"");
            sb.Append("mobile.this.Immunity = \"nonmagic\"");
            sb.Append("mobile.this.Attacks = \"kick firebreath\"");
            sb.Append("mobile.this.Defenses = \"dodge\"");

            sb.Append("newProg = LCreateMudProg(\"greet_prog\");");
            sb.Append("mprog.this = newProg;");
            sb.Append("mprog.this.ArgList = \"100\";");
            sb.Append("mprog.this.Script = [[LMobCommand(\"cac\");LMobSay(\"Now your soul shall be mine!\");]];");
            sb.Append("mobile.this:AddMudProg(mprog.this);");

            sb.Append("newShop = LCreateShop(130, 90, 7, 21);");
            sb.Append("shop.this = newShop;");
            sb.Append("shop.this:AddItemType(\"armor\");");
            sb.Append("shop.this:AddItemType(\"weapon\");");
            sb.Append("mobile.this:AddShop(shop.this);");

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
            RepositoryManager dbMgr = new RepositoryManager(mockKernel.Object, new Mock<ILogManager>().Object);

            LuaMobFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);
            LuaCreateFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);

            dbMgr.MOBILETEMPLATES.CastAs<Repository<long, MobileTemplate>>().Clear();

            _proxy = new LuaInterfaceProxy();

            var luaFuncRepo = LuaHelper.Register(typeof(LuaMobFunctions), null);
            luaFuncRepo = LuaHelper.Register(typeof(LuaCreateFunctions), luaFuncRepo);
            _proxy.RegisterFunctions(luaFuncRepo);

            luaMgr.InitializeLuaProxy(_proxy);
        }

        [Test]
        public void LuaCreateMobileTest()
        {
            var result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(801));
            Assert.That(result.Name, Is.EqualTo("nightmare"));
            Assert.That(result.ShortDescription, Is.EqualTo("A pitch-black nightmare"));
            Assert.That(result.LongDescription, Is.EqualTo("A nightmare is here, kicking at you with its flaming hooves."));
            Assert.That(result.Description.StartsWith("The nightmare is a wholly evil being,"), Is.True);
            Assert.That(result.Race, Is.EqualTo("magical"));
            Assert.That(result.Class, Is.EqualTo("warrior"));
            Assert.That(result.Position, Is.EqualTo("standing"));
            Assert.That(result.DefensivePosition, Is.EqualTo("standing"));
            Assert.That(result.GetGender(), Is.EqualTo(GenderTypes.Neuter));
            Assert.That(result.SpecFun, Is.EqualTo("DoSpecCastMage"));
            Assert.That(result.Speaks, Is.EqualTo("magical"));
            Assert.That(result.Speaking, Is.EqualTo("magical"));
            Assert.That(result.BodyParts, Is.EqualTo("head legs heart guts feet"));
            Assert.That(result.Resistance, Is.EqualTo("sleep charm hold"));
            Assert.That(result.Susceptibility, Is.EqualTo("fire blunt"));
            Assert.That(result.Immunity, Is.EqualTo("nonmagic"));
            Assert.That(result.Attacks, Is.EqualTo("kick firebreath"));
            Assert.That(result.Defenses, Is.EqualTo("dodge"));
        }

        [Test]
        public void LuaCreateMobile_SetStats1_Test()
        {
            var result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetStatistic<int>(StatisticTypes.Alignment), Is.EqualTo(-950));
            Assert.That(result.Level, Is.EqualTo(18));
            Assert.That(result.GetStatistic<int>(StatisticTypes.ToHitArmorClass0), Is.EqualTo(2));
            Assert.That(result.GetStatistic<int>(StatisticTypes.ArmorClass), Is.EqualTo(-2));
            Assert.That(result.GetStatistic<int>(StatisticTypes.Coin), Is.EqualTo(6000));
            Assert.That(result.GetStatistic<int>(StatisticTypes.Experience), Is.EqualTo(32000));
        }

        [Test]
        public void LuaCreateMobile_SetStats2_Test()
        {
            var result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.HitDice, Is.Not.Null);
            Assert.That(result.HitDice.NumberOf, Is.EqualTo(18));
            Assert.That(result.HitDice.SizeOf, Is.EqualTo(18));
            Assert.That(result.HitDice.Bonus, Is.EqualTo(180));
        }

        [Test]
        public void LuaCreateMobile_SetStats3_Test()
        {
            var result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.DamageDice, Is.Not.Null);
            Assert.That(result.DamageDice.NumberOf, Is.EqualTo(5));
            Assert.That(result.DamageDice.SizeOf, Is.EqualTo(3));
            Assert.That(result.DamageDice.Bonus, Is.EqualTo(10));
        }

        [Test]
        public void LuaCreateMobile_SetStats4_Test()
        {
            var result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetStatistic<int>(StatisticTypes.Height), Is.EqualTo(50));
            Assert.That(result.GetStatistic<int>(StatisticTypes.Weight), Is.EqualTo(100));
            Assert.That(result.GetStatistic<int>(StatisticTypes.NumberOfAttacks), Is.EqualTo(2));
            Assert.That(result.GetStatistic<int>(StatisticTypes.Hitroll), Is.EqualTo(5));
            Assert.That(result.GetStatistic<int>(StatisticTypes.Damroll), Is.EqualTo(6));
        }

        [Test]
        public void LuaCreateMobile_SetAttribs_Test()
        {
            var result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetStatistic<int>(StatisticTypes.PermanentStrength), Is.EqualTo(11));
            Assert.That(result.GetStatistic<int>(StatisticTypes.PermanentIntelligence), Is.EqualTo(12));
            Assert.That(result.GetStatistic<int>(StatisticTypes.PermanentWisdom), Is.EqualTo(13));
            Assert.That(result.GetStatistic<int>(StatisticTypes.PermanentDexterity), Is.EqualTo(14));
            Assert.That(result.GetStatistic<int>(StatisticTypes.PermanentConstitution), Is.EqualTo(15));
            Assert.That(result.GetStatistic<int>(StatisticTypes.PermanentCharisma), Is.EqualTo(16));
            Assert.That(result.GetStatistic<int>(StatisticTypes.PermanentLuck), Is.EqualTo(17));
        }

        [Test]
        public void LuaCreateMobile_SetSaves_Test()
        {
            var result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.SavingThrows, Is.Not.Null);
            Assert.That(result.SavingThrows.SaveVsPoisonDeath, Is.EqualTo(3));
            Assert.That(result.SavingThrows.SaveVsWandRod, Is.EqualTo(5));
            Assert.That(result.SavingThrows.SaveVsParalysisPetrify, Is.EqualTo(3));
            Assert.That(result.SavingThrows.SaveVsBreath, Is.EqualTo(5));
            Assert.That(result.SavingThrows.SaveVsSpellStaff, Is.EqualTo(3));
        }

        [Test]
        public void LuaCreateMobile_MudProgs_Test()
        {
            var result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.MudProgs.Count(), Is.EqualTo(1));
            Assert.That(result.MudProgs.First().Type, Is.EqualTo(MudProgTypes.Greet));
            Assert.That(result.MudProgs.First().ArgList, Is.EqualTo("100"));
            Assert.That(result.MudProgs.First().Script,
                Is.EqualTo("LMobCommand(\"cac\");LMobSay(\"Now your soul shall be mine!\");"));
        }

        [Test]
        public void LuaCreateMobile_Shop_Test()
        {
            var result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Shop, Is.Not.Null);
            Assert.That(result.Shop.ShopType, Is.EqualTo(ShopTypes.Item));
            Assert.That(result.Shop.OpenHour, Is.EqualTo(7));
            Assert.That(result.Shop.CloseHour, Is.EqualTo(21));
            Assert.That(result.Shop.ItemTypes.Count(), Is.EqualTo(2));
            Assert.That(result.Shop.ItemTypes.Contains(ItemTypes.Armor), Is.True);
            Assert.That(result.Shop.ItemTypes.Contains(ItemTypes.Weapon), Is.True);

            var itemShop = result.Shop.CastAs<ItemShopData>();
            Assert.That(itemShop, Is.Not.Null);
            Assert.That(itemShop.ProfitBuy, Is.EqualTo(130));
            Assert.That(itemShop.ProfitSell, Is.EqualTo(90));
        }

        /*[Test]
        [ExpectedException(typeof(DuplicateEntryException))]
        public void LuaCreateMobileDuplicateTest()
        {
            LuaMobFunctions.LuaProcessMob(GetMobLuaScript());
            LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

            Assert.Fail("Unit test expected a DuplicateEntryException to be thrown!");
        }*/

        [Test]
        public void Create()
        {
            var repo = new MobileRepository();

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
            var repo = new MobileRepository();

            var source = repo.Create(1, "Test");
            source.LongDescription = "This is a test";

            var cloned = repo.Create(2, 1, "Test2");

            Assert.That(cloned, Is.Not.Null);
            Assert.That(cloned.ID, Is.EqualTo(2));
            Assert.That(cloned.Name, Is.EqualTo("Test2"));
            Assert.That(cloned.ShortDescription, Is.EqualTo("A newly created Test2"));
            Assert.That(cloned.LongDescription, Is.EqualTo(source.LongDescription));
            Assert.That(repo.Contains(2), Is.True);
        }

        [Test]
        public void Create_ThrowsException()
        {
            var repo = new MobileRepository();

            Assert.Throws<ArgumentException>(() => repo.Create(1, ""));
        }

        [Test]
        public void Create_ThrowsException_InvalidVnum()
        {
            var repo = new MobileRepository();

            Assert.Throws<ArgumentException>(() => repo.Create(0, "Test"));
        }

        [Test]
        public void Create_DuplicateVnum()
        {
            var repo = new MobileRepository();

            repo.Create(1, "Test");
            Assert.Throws<DuplicateIndexException>(() => repo.Create(1, "Test2"));
        }

        [Test]
        public void Create_Clone_InvalidCloneVnum()
        {
            var repo = new MobileRepository();

            Assert.Throws<ArgumentException>(() => repo.Create(1, 1, "Test"));
        }

        [Test]
        public void Create_Clone_MissingCloneMob()
        {
            var repo = new MobileRepository();

            repo.Create(1, "Test");
            Assert.Throws<InvalidDataException>(() => repo.Create(2, 5, "Test2"));
        }
    }
}
