using FakeItEasy;
using FluentAssertions;
using LuaInterface.Exceptions;
using Ninject;
using Realm.Library.Common.Objects;
using Realm.Library.Lua;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Shops;
using SmaugCS.Logging;
using SmaugCS.Lua;
using SmaugCS.Repository;
using System.Linq;
using System.Text;
using Test.Common;
using Xunit;

namespace SmaugCS.Tests
{
    [Collection(CollectionDefinitions.NonParallelCollection)]
    public class LuaCreateFunctionTests
    {
        private static LuaManager LuaMgr { get; set; }

        public LuaCreateFunctionTests()
        {
            ILogManager mockLogManager = A.Fake<ILogManager>();
            A.CallTo(() => mockLogManager.Boot(A<string>.Ignored, A<object[]>.Ignored));

            const string dataPath = "D://Projects//SmaugCS//trunk//data";

            IKernel mockKernel = A.Fake<IKernel>();

            LuaMgr = new LuaManager(mockKernel, mockLogManager.LogWrapper);

            RepositoryManager dbMgr = new RepositoryManager(mockKernel, mockLogManager);

            LuaGetFunctions.InitializeReferences(LuaMgr, dbMgr, dataPath);
            LuaCreateFunctions.InitializeReferences(LuaMgr, dbMgr, mockLogManager);

            var luaProxy = new LuaInterfaceProxy();

            var luaFuncRepo = LuaHelper.Register(typeof(LuaCreateFunctions), null);
            luaProxy.RegisterFunctions(luaFuncRepo);

            LuaMgr.InitializeLuaProxy(luaProxy);
        }

        #region MudProg
        private static string GetMudProgLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newProg = LCreateMudProg(\"greet_prog\");");
            sb.Append("mprog.this = newProg;");
            sb.Append("mprog.this.ArgList = \"100\";");
            sb.Append("mprog.this.Script = [[LMobCommand(\"cac\");LMobSay(\"Now your soul shall be mine!\");]];");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateMudProgTest()
        {
            LuaMgr.Proxy.DoString(GetMudProgLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<MudProgData>();

            result.Should().NotBeNull();
            result.Type.Should().Be(MudProgTypes.Greet);
            result.ArgList.Should().Be("100");
            result.Script.Should().Be("LMobCommand(\"cac\");LMobSay(\"Now your soul shall be mine!\");");
        }
        #endregion

        #region Shop
        private static string GetShopLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newShop = LCreateShop(130, 90, 7, 21);");
            sb.Append("shop.this = newShop;");
            sb.Append("shop.this:AddItemType(\"armor\");");
            sb.Append("shop.this:AddItemType(\"weapon\");");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateShopTest()
        {
            LuaMgr.Proxy.DoString(GetShopLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<ShopData>();

            result.Should().NotBeNull();
            result.ShopType.Should().Be(ShopTypes.Item);
            result.OpenHour.Should().Be(7);
            result.CloseHour.Should().Be(21);
            result.ItemTypes.Count().Should().Be(2);
            result.ItemTypes.Contains(ItemTypes.Armor).Should().BeTrue();
            result.ItemTypes.Contains(ItemTypes.Weapon).Should().BeTrue();

            var itemShop = result.CastAs<ItemShopData>();
            itemShop.Should().NotBeNull();
            itemShop.ProfitBuy.Should().Be(130);
            itemShop.ProfitSell.Should().Be(90);
        }
        #endregion

        #region Reset
        private static string GetResetLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newReset = LCreateReset(\"mob\", 0, 100, 104, 1);");
            sb.Append("reset.this = newReset;");
            sb.Append("reset.this:AddReset(\"give\", 0, 110, 0, 0);");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateResetTest()
        {
            LuaMgr.Proxy.DoString(GetResetLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<ResetData>();

            result.Should().NotBeNull();
            result.Type.Should().Be(ResetTypes.Mob);
            result.Args.ToList()[0].Should().Be(100);
            result.Args.ToList()[1].Should().Be(104);
            result.Args.ToList()[2].Should().Be(1);
        }

        [Fact]
        public void LuaCreateReset_AddReset_Test()
        {
            LuaMgr.Proxy.DoString(GetResetLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<ResetData>();

            result.Should().NotBeNull();
            result.Resets.Count.Should().Be(1);
            result.Resets.ToList()[0].Type.Should().Be(ResetTypes.Give);
            result.Resets.ToList()[0].Args.ToList()[0].Should().Be(110);
        }
        #endregion

        #region Liquid
        private static string GetLiquidLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newLiquid = LCreateLiquid(4, \"dark ale\");");
            sb.Append("liquid.this = newLiquid;");
            sb.Append("liquid.this.ShortDescription = \"dark ale\";");
            sb.Append("liquid.this.Color = \"dark brown\";");
            sb.Append("liquid.this:SetType(\"alcohol\");");
            sb.Append("liquid.this:AddMods(1, 2, 5, 7);");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateLiquid_Test()
        {
            LuaMgr.Proxy.DoString(GetLiquidLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<LiquidData>();

            result.Should().NotBeNull();
            result.ID.Should().Be(4);
            result.Name.Should().Be("dark ale");
            result.ShortDescription.Should().Be("dark ale");
            result.Color.Should().Be("dark brown");
            result.Type.Should().Be(LiquidTypes.Alcohol);
            result.GetMod(ConditionTypes.Drunk).Should().Be(1);
            result.GetMod(ConditionTypes.Full).Should().Be(2);
            result.GetMod(ConditionTypes.Thirsty).Should().Be(5);
            result.GetMod(ConditionTypes.Bloodthirsty).Should().Be(7);
        }
        #endregion

        #region Herb
        private static string GetHerbLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newHerb = LCreateHerb(1, \"black gwyvel\", \"herb\");");
            sb.Append("herb.this = newHerb;");
            sb.Append("herb.this.Rounds = 12;");
            sb.Append("herb.this.DamageMessage = \"smoke\";");
            sb.Append("herb.this:SetTargetByValue(1);");
            sb.Append("herb.this.MinimumPosition = 7;");
            sb.Append("herb.this.Slot = 1;");
            sb.Append("herb.this.HitVictimMessage = \"You start to cough and choke!\";");
            sb.Append("herb.this.SkillFunctionName = \"spell_smaug\";");
            sb.Append("newAffect = LCreateSmaugAffect(\"\", 13, \"-10\", 0);");
            sb.Append("affect.this = newAffect;");
            sb.Append("herb.this:AddAffect(affect.this);");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateHerb_Test()
        {
            LuaMgr.Proxy.DoString(GetHerbLuaScript());
            var result = LuaCreateFunctions.GetLastObject(typeof(HerbData)).CastAs<HerbData>();

            result.Should().NotBeNull();
            result.Rounds.Should().Be(12);
            result.MinimumPosition.Should().Be(9);
            result.Slot.Should().Be(1);
            result.HitVictimMessage.Should().Be("You start to cough and choke!");
            result.Target.Should().Be(TargetTypes.OffensiveCharacter);
            result.SkillFunctionName.Should().Be("spell_smaug");
            result.DamageMessage.Should().Be("smoke");
        }
        #endregion

        #region Skill
        private static string GetSkillLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newSkill = LCreateSkill(1, \"Wrath of Dominus\", \"spell\");");
            sb.Append("skill.this = newSkill;");
            sb.Append("skill.this:SetFlags(\"secretskill noscribe\");");
            sb.Append("skill.this:SetTargetByValue(2);");
            sb.Append("skill.this.MinimumPosition = 109;");
            sb.Append("skill.this.Slot = 246;");
            sb.Append("skill.this.MinimumMana = 10;");
            sb.Append("skill.this.MinimumLevel = 51;");
            sb.Append("skill.this.SkillFunctionName = \"spell_smaug\";");
            sb.Append("newAffect = LCreateSmaugAffect(\"10\", 13, \"-75\", -1);");
            sb.Append("affect.this = newAffect;");
            sb.Append("skill.this:AddAffect(affect.this);");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateSkill_Test()
        {
            LuaMgr.Proxy.DoString(GetSkillLuaScript());
            var result = LuaCreateFunctions.GetLastObject(typeof(SkillData)).CastAs<SkillData>();

            result.Should().NotBeNull();
            result.Flags.Should().Be(33792);
            result.MinimumPosition.Should().Be(9);
            result.Slot.Should().Be(246);
            result.MinimumMana.Should().Be(10);
            result.MinimumLevel.Should().Be(51);
            result.Target.Should().Be(TargetTypes.DefensiveCharacter);
            result.SkillFunctionName.Should().Be("spell_smaug");
        }
        #endregion

        #region SmaugAffect
        private static string GetSmaugAffectLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newAffect = LCreateSmaugAffect(\"10\", 13, \"-10\", 258)");
            sb.Append("affect.this = newAffect;");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateSmaugAffect_Test()
        {
            LuaMgr.Proxy.DoString(GetSmaugAffectLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<SmaugAffect>();

            result.Should().NotBeNull();
            result.Duration.Should().Be("10");
            result.Location.Should().Be(13);
            result.Modifier.Should().Be("-10");
            result.Flags.Should().Be(258);
        }
        #endregion

        #region SpecFun
        private static string GetSpecFunLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newSpecFun = LCreateSpecFun(\"spec_breath_any\");");
            sb.Append("specfun.this = newSpecFun;");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateSpecialFunction_Test()
        {
            LuaMgr.Proxy.DoString(GetSpecFunLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<SpecialFunction>();

            result.Should().NotBeNull();
            result.Name.Should().Be("spec_breath_any");
        }
        #endregion

        #region Command
        private static string GetCommandLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newCommand = LCreateCommand(\"auction\", \"do_auction\", 104, 5, 1, 1);");
            sb.Append("command.this = newCommand;");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateCommand_Test()
        {
            LuaMgr.Proxy.DoString(GetCommandLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<CommandData>();

            result.Should().NotBeNull();
            result.Name.Should().Be("auction");
            result.FunctionName.Should().Be("do_auction");
            result.Position.Should().Be(4);
            result.Level.Should().Be(5);
            result.Log.Should().Be(LogAction.Normal);
            result.Flags.Should().Be(1);
        }
        #endregion

        #region Social
        private static string GetSocialLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newSocial = LCreateSocial(\"accuse\")");
            sb.Append("social.this = newSocial;");
            sb.Append("social.this.CharNoArg = \"Accuse whom?\";");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateSocial_Test()
        {
            LuaMgr.Proxy.DoString(GetSocialLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<SocialData>();

            result.Should().NotBeNull();
            result.Name.Should().Be("accuse");
            result.CharNoArg.Should().Be("Accuse whom?");
        }
        #endregion

        #region SpellComponent
        private static string GetSpellCommponentLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newComponent = LCreateSpellComponent(\"V\", \"65\", \"@\");");
            sb.Append("component.this = newComponent;");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateSpellComponent_Test()
        {
            LuaMgr.Proxy.DoString(GetSpellCommponentLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<SpellComponent>();

            result.Should().NotBeNull();
            result.RequiredType.Should().Be(ComponentRequiredTypes.ItemVnum);
            result.RequiredData.Should().Be("65");
            result.OperatorType.Should().Be(ComponentOperatorTypes.DecreaseValue0);
        }
        #endregion

        #region Class
        private static string GetClassLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newClass = LCreateClass(\"Warrior\", 3);");
            sb.Append("class.this = newClass;");
            sb.Append("class.this.SkillAdept = 85;");
            sb.Append("class.this.ToHitArmorClass0 = 18;");
            sb.Append("class.this.ToHitArmorClass32 = 6;");
            sb.Append("class.this.MinimumHealthGain = 11;");
            sb.Append("class.this.MaximumHealthGain = 15;");
            sb.Append("class.this.BaseExperience = 1150;");
            sb.Append("class.this:SetPrimaryAttribute(\"permanentstrength\");");
            sb.Append("class.this:SetSecondaryAttribute(\"permanentconstitution\");");
            sb.Append("class.this:SetDeficientAttribute(\"permanentcharisma\");");
            sb.Append("class.this:SetType(\"warrior\");");
            sb.Append("class.this:AddSkill(\"aggressive style\", 20, 50);");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateClassTest()
        {
            LuaMgr.Proxy.DoString(GetClassLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<ClassData>();

            result.Should().NotBeNull();
            result.SkillAdept.Should().Be(85);
            result.ToHitArmorClass0.Should().Be(18);
            result.ToHitArmorClass32.Should().Be(6);
            result.MinimumHealthGain.Should().Be(11);
            result.MaximumHealthGain.Should().Be(15);
            result.BaseExperience.Should().Be(1150);
        }

        [Fact]
        public void LuaCreateClass_SetPrimaryAttribute_Test()
        {
            LuaMgr.Proxy.DoString(GetClassLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<ClassData>();

            result.Should().NotBeNull();
            result.PrimaryAttribute.Should().Be(StatisticTypes.PermanentStrength);
        }

        [Fact]
        public void LuaCreateClass_SetSecondaryAttribute_Test()
        {
            LuaMgr.Proxy.DoString(GetClassLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<ClassData>();

            result.Should().NotBeNull();
            result.SecondaryAttribute.Should().Be(StatisticTypes.PermanentConstitution);
        }

        [Fact]
        public void LuaCreateClass_SetDeficientAttribute_Test()
        {
            LuaMgr.Proxy.DoString(GetClassLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<ClassData>();

            result.Should().NotBeNull();
            result.DeficientAttribute.Should().Be(StatisticTypes.PermanentCharisma);
        }

        [Fact]
        public void LuaCreateClass_SetType_Test()
        {
            LuaMgr.Proxy.DoString(GetClassLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<ClassData>();

            result.Should().NotBeNull();
            result.Type.Should().Be(ClassTypes.Warrior);
        }

        [Fact]
        public void LuaCreateClass_AddSkill_Test()
        {
            LuaMgr.Proxy.DoString(GetClassLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<ClassData>();

            result.Should().NotBeNull();

            var skillAdept = result.Skills.FirstOrDefault(x => x.Skill.Equals("aggressive style"));

            skillAdept.Should().NotBeNull();
            skillAdept.Level.Should().Be(20);
            skillAdept.Adept.Should().Be(50);
        }

        [Fact]
        public void LuaCreateClass_AddSkill_Duplicate_Test()
        {
            var script = GetClassLuaScript();
            script += "class.this:AddSkill(\"aggressive style\", 20, 50);";

            Assert.Throws<LuaScriptException>(() => LuaMgr.Proxy.DoString(script));
        }
        #endregion

        #region Race
        private static string GetRaceLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newRace = LCreateRace(\"Human\", 0);");
            sb.Append("race.this = newRace;");
            sb.Append("race.this.ClassRestriction = 512;");
            sb.Append("race.this.Language = 1;");
            sb.Append("race.this.MinimumAlignment = -1000;");
            sb.Append("race.this.MaximumAlignment = 1000;");
            sb.Append("race.this.ExperienceMultiplier = 100;");
            sb.Append("race.this.Height = 66;");
            sb.Append("race.this.Weight = 150;");
            sb.Append("race.this:AddWhereName(\"<used as light>     \");");
            sb.Append("race.this:AddWhereName(\"<worn on finger>    \");");
            sb.Append("race.this:AddAffectedBy(\"infrared\");");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateRaceTest()
        {
            LuaMgr.Proxy.DoString(GetRaceLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<RaceData>();

            result.Should().NotBeNull();
            result.ClassRestriction.Should().Be(512);
            result.Language.Should().Be(1);
            result.MinimumAlignment.Should().Be(-1000);
            result.MaximumAlignment.Should().Be(1000);
            result.ExperienceMultiplier.Should().Be(100);
            result.Height.Should().Be(66);
            result.Weight.Should().Be(150);
        }

        [Fact]
        public void LuaCreateRace_AddWhereName_Test()
        {
            LuaMgr.Proxy.DoString(GetRaceLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<RaceData>();

            result.Should().NotBeNull();
            result.WhereNames.ToList()[0].Should().Be("<used as light>     ");
            result.WhereNames.ToList()[1].Should().Be("<worn on finger>    ");
        }

        [Fact]
        public void LuaCreateRace_AddAffectedBy_Test()
        {
            LuaMgr.Proxy.DoString(GetRaceLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<RaceData>();

            result.Should().NotBeNull();
            result.AffectedBy.IsSet((int)AffectedByTypes.Infrared).Should().BeTrue();
        }
        #endregion

        #region Clan
        private static string GetClanLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newClan = LCreateClan(\"Guild of Augurers\");");
            sb.Append("clan.this = newClan;");
            sb.Append("clan.this:SetTypeByValue(14);");
            sb.Append("clan.this.Class = 7;");
            sb.Append("clan.this.Board = 21433;");
            sb.Append("clan.this.RecallRoom = 21430;");
            sb.Append("clan.this.Storeroom = 21434;");
            return sb.ToString();
        }

        // TODO Fix
        //[Fact]
        //public void LuaCreateClanTest()
        //{
        //    LuaMgr.Proxy.DoString(GetClanLuaScript());
        //    var result = LuaCreateFunctions.LastObject.CastAs<ClanData>();

        //    result.Should().NotBeNull();
        //    result.Class.Should().Be(7);
        //    result.Board.Should().Be(21433);
        //    result.RecallRoom.Should().Be(21430);
        //    result.StoreRoom.Should().Be(21434);
        //    result.ClanType.Should().Be(ClanTypes.Guild);
        //}
        #endregion

        #region Deity
        private static string GetDeityLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newDeity = LCreateDeity(\"Maron\");");
            sb.Append("deity.this = newDeity;");
            sb.Append("deity.this.Description = \"This is a deity description\";");
            sb.Append("deity.this.Worshippers = 1673;");
            sb.Append("deity.this:AddFieldValue(\"Flee\", -2);");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateDeityTest()
        {
            LuaMgr.Proxy.DoString(GetDeityLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<DeityData>();

            result.Should().NotBeNull();
            result.Name.Should().Be("Maron");
            result.Description.Should().Be("This is a deity description");
            result.Worshippers.Should().Be(1673);
            result.GetFieldValue(DeityFieldTypes.Flee).Should().Be(-2);
        }
        #endregion

        #region Language
        private static string GetLanguageLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newLang = LCreateLanguage(\"elvish\", \"elven\");");
            sb.Append("lang.this = newLang;");
            sb.Append("lang.this:AddPreConversion(\"the hour\", \"lumenn\");");
            sb.Append("lang.this:AddPreConversion(\"our meeting\", \"omentielvo\");");
            sb.Append("lang.this:AddPreConversion(\"shines\", \"sila\");");
            sb.Append("lang.this.Alphabet = \"iqqdakvtujfwghepcrslybszoz\";");
            sb.Append("lang.this:AddPostConversion(\"rr\", \"r\");");
            sb.Append("lang.this:AddPostConversion(\"qq\", \"q\");");
            return sb.ToString();
        }

        [Fact]
        public void LuaCreateLanguageTest()
        {
            LuaMgr.Proxy.DoString(GetLanguageLuaScript());
            var result = LuaCreateFunctions.LastObject.CastAs<Language.LanguageData>();

            result.Should().NotBeNull();
            result.Name.Should().Be("elvish");
            result.Alphabet.Should().Be("iqqdakvtujfwghepcrslybszoz");

            result.PreConversion.Count().Should().Be(3);
            result.PreConversion.First().Should().NotBeNull();
            result.PreConversion.First().OldValue.Should().Be("the hour");
            result.PreConversion.First().NewValue.Should().Be("lumenn");

            result.Conversion.Count().Should().Be(2);
            result.Conversion.First().Should().NotBeNull();
            result.Conversion.First().OldValue.Should().Be("rr");
            result.Conversion.First().NewValue.Should().Be("r");
        }
        #endregion

    }
}
