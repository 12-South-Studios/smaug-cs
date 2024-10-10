using FakeItEasy;
using FluentAssertions;
using Library.Common;
using Library.Common.Logging;
using Library.Common.Objects;
using Library.Lua;
using Patterns.Repository;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.DAL;
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
using SmaugCS.LuaHelpers;
using Test.Common;
using Xunit;

namespace SmaugCS.Tests.Repositories;

[Collection(CollectionDefinitions.NonParallelCollection)]
public class MobileRepositoryTests
{
  private static string GetMobLuaScript()
  {
    StringBuilder sb = new();
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

  public MobileRepositoryTests()
  {
    IDbContext mockCtx = A.Fake<IDbContext>();
    ILogWrapper mockLogger = A.Fake<ILogWrapper>();
    ITimer mockTimer = A.Fake<ITimer>();

    LuaManager luaMgr = new(mockLogger);
    LogManager logMgr = new(mockLogger, mockTimer, mockCtx);
    RepositoryManager dbMgr = new(A.Fake<ILogManager>());

    LuaMobFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);
    LuaCreateFunctions.InitializeReferences(luaMgr, dbMgr, logMgr);

    dbMgr.MOBILETEMPLATES.CastAs<Repository<long, MobileTemplate>>().Clear();

    _proxy = new LuaInterfaceProxy();

    LuaFunctionRepository luaFuncRepo = LuaHelper.Register(typeof(LuaMobFunctions), null);
    luaFuncRepo = LuaHelper.Register(typeof(LuaCreateFunctions), luaFuncRepo);
    _proxy.RegisterFunctions(luaFuncRepo);

    luaMgr.InitializeLuaProxy(_proxy);
  }

  [Fact]
  public void LuaCreateMobileTest()
  {
    MobileTemplate result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

    result.Should().NotBeNull();
    result.Id.Should().Be(801);
    result.Name.Should().Be("nightmare");
    result.ShortDescription.Should().Be("A pitch-black nightmare");
    result.LongDescription.Should().Be("A nightmare is here, kicking at you with its flaming hooves.");
    result.Description.StartsWith("The nightmare is a wholly evil being,").Should().BeTrue();
    result.Race.Should().Be("magical");
    result.Class.Should().Be("warrior");
    result.Position.Should().Be("standing");
    result.DefensivePosition.Should().Be("standing");
    result.GetGender().Should().Be(GenderTypes.Neuter);
    result.SpecFun.Should().Be("DoSpecCastMage");
    result.Speaks.Should().Be("magical");
    result.Speaking.Should().Be("magical");
    result.BodyParts.Should().Be("head legs heart guts feet");
    result.Resistance.Should().Be("sleep charm hold");
    result.Susceptibility.Should().Be("fire blunt");
    result.Immunity.Should().Be("nonmagic");
    result.Attacks.Should().Be("kick firebreath");
    result.Defenses.Should().Be("dodge");
  }

  [Fact]
  public void LuaCreateMobile_SetStats1_Test()
  {
    MobileTemplate result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

    result.Should().NotBeNull();
    result.GetStatistic<int>(StatisticTypes.Alignment).Should().Be(-950);
    result.Level.Should().Be(18);
    result.GetStatistic<int>(StatisticTypes.ToHitArmorClass0).Should().Be(2);
    result.GetStatistic<int>(StatisticTypes.ArmorClass).Should().Be(-2);
    result.GetStatistic<int>(StatisticTypes.Coin).Should().Be(6000);
    result.GetStatistic<int>(StatisticTypes.Experience).Should().Be(32000);
  }

  [Fact]
  public void LuaCreateMobile_SetStats2_Test()
  {
    MobileTemplate result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

    result.Should().NotBeNull();
    result.HitDice.Should().NotBeNull();
    result.HitDice.NumberOf.Should().Be(18);
    result.HitDice.SizeOf.Should().Be(18);
    result.HitDice.Bonus.Should().Be(180);
  }

  [Fact]
  public void LuaCreateMobile_SetStats3_Test()
  {
    MobileTemplate result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

    result.Should().NotBeNull();
    result.DamageDice.Should().NotBeNull();
    result.DamageDice.NumberOf.Should().Be(5);
    result.DamageDice.SizeOf.Should().Be(3);
    result.DamageDice.Bonus.Should().Be(10);
  }

  [Fact]
  public void LuaCreateMobile_SetStats4_Test()
  {
    MobileTemplate result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

    result.Should().NotBeNull();
    result.GetStatistic<int>(StatisticTypes.Height).Should().Be(50);
    result.GetStatistic<int>(StatisticTypes.Weight).Should().Be(100);
    result.GetStatistic<int>(StatisticTypes.NumberOfAttacks).Should().Be(2);
    result.GetStatistic<int>(StatisticTypes.Hitroll).Should().Be(5);
    result.GetStatistic<int>(StatisticTypes.Damroll).Should().Be(6);
  }

  [Fact]
  public void LuaCreateMobile_SetAttribs_Test()
  {
    MobileTemplate result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

    result.Should().NotBeNull();
    result.GetStatistic<int>(StatisticTypes.PermanentStrength).Should().Be(11);
    result.GetStatistic<int>(StatisticTypes.PermanentIntelligence).Should().Be(12);
    result.GetStatistic<int>(StatisticTypes.PermanentWisdom).Should().Be(13);
    result.GetStatistic<int>(StatisticTypes.PermanentDexterity).Should().Be(14);
    result.GetStatistic<int>(StatisticTypes.PermanentConstitution).Should().Be(15);
    result.GetStatistic<int>(StatisticTypes.PermanentCharisma).Should().Be(16);
    result.GetStatistic<int>(StatisticTypes.PermanentLuck).Should().Be(17);
  }

  [Fact]
  public void LuaCreateMobile_SetSaves_Test()
  {
    MobileTemplate result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

    result.Should().NotBeNull();
    result.SavingThrows.Should().NotBeNull();
    result.SavingThrows.SaveVsPoisonDeath.Should().Be(3);
    result.SavingThrows.SaveVsWandRod.Should().Be(5);
    result.SavingThrows.SaveVsParalysisPetrify.Should().Be(3);
    result.SavingThrows.SaveVsBreath.Should().Be(5);
    result.SavingThrows.SaveVsSpellStaff.Should().Be(3);
  }

  [Fact]
  public void LuaCreateMobile_MudProgs_Test()
  {
    MobileTemplate result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

    result.Should().NotBeNull();
    result.MudProgs.Count().Should().Be(1);
    result.MudProgs.First().Type.Should().Be(MudProgTypes.Greet);
    result.MudProgs.First().ArgList.Should().Be("100");
    result.MudProgs.First().Script.Should().Be("LMobCommand(\"cac\");LMobSay(\"Now your soul shall be mine!\");");
  }

  [Fact]
  public void LuaCreateMobile_Shop_Test()
  {
    MobileTemplate result = LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

    result.Should().NotBeNull();
    result.Shop.Should().NotBeNull();
    result.Shop.ShopType.Should().Be(ShopTypes.Item);
    result.Shop.OpenHour.Should().Be(7);
    result.Shop.CloseHour.Should().Be(21);
    result.Shop.ItemTypes.Count().Should().Be(2);
    result.Shop.ItemTypes.Contains(ItemTypes.Armor).Should().BeTrue();
    result.Shop.ItemTypes.Contains(ItemTypes.Weapon).Should().BeTrue();

    ItemShopData itemShop = result.Shop.CastAs<ItemShopData>();
    itemShop.Should().NotBeNull();
    itemShop.ProfitBuy.Should().Be(130);
    itemShop.ProfitSell.Should().Be(90);
  }

  /*[Fact]
  [ExpectedException(typeof(DuplicateEntryException))]
  public void LuaCreateMobileDuplicateTest()
  {
      LuaMobFunctions.LuaProcessMob(GetMobLuaScript());
      LuaMobFunctions.LuaProcessMob(GetMobLuaScript());

      Assert.Fail("Unit test expected a DuplicateEntryException to be thrown!");
  }*/

  [Fact]
  public void Create()
  {
    MobileRepository repo = new();

    MobileTemplate actual = repo.Create(1, "Test");

    actual.Should().NotBeNull();
    actual.Id.Should().Be(1);
    actual.Name.Should().Be("Test");
    actual.ShortDescription.Should().Be("A newly created Test");
    repo.Contains(1).Should().BeTrue();
  }

  [Fact]
  public void Create_CloneObject()
  {
    MobileRepository repo = new();

    MobileTemplate source = repo.Create(1, "Test");
    source.LongDescription = "This is a test";

    MobileTemplate cloned = repo.Create(2, 1, "Test2");

    cloned.Should().NotBeNull();
    cloned.Id.Should().Be(2);
    cloned.Name.Should().Be("Test2");
    cloned.ShortDescription.Should().Be("A newly created Test2");
    cloned.LongDescription.Should().Be(source.LongDescription);
    repo.Contains(2).Should().BeTrue();
  }

  [Fact]
  public void Create_ThrowsException()
  {
    MobileRepository repo = new();

    Assert.Throws<ArgumentException>(() => repo.Create(1, ""));
  }

  [Fact]
  public void Create_ThrowsException_InvalidVnum()
  {
    MobileRepository repo = new();

    Assert.Throws<ArgumentException>(() => repo.Create(0, "Test"));
  }

  [Fact]
  public void Create_DuplicateVnum()
  {
    MobileRepository repo = new();

    repo.Create(1, "Test");
    Assert.Throws<DuplicateIndexException>(() => repo.Create(1, "Test2"));
  }

  [Fact]
  public void Create_Clone_InvalidCloneVnum()
  {
    MobileRepository repo = new();

    Assert.Throws<ArgumentException>(() => repo.Create(1, 1, "Test"));
  }

  [Fact]
  public void Create_Clone_MissingCloneMob()
  {
    MobileRepository repo = new();

    repo.Create(1, "Test");
    Assert.Throws<InvalidDataException>(() => repo.Create(2, 5, "Test2"));
  }
}