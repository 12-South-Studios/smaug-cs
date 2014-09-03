using System.IO;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Realm.Library.Common;
using Realm.Library.Lua;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Organizations;
using SmaugCS.Data.Shops;
using SmaugCS.Logging;
using SmaugCS.LuaHelpers;
using SmaugCS.Managers;

namespace SmaugCS.Tests
{
	[TestFixture]
	public class LuaCreateFunctionTests
	{
        public static LuaManager LuaMgr { get; set; }

		[SetUp]
		public void OnSetup()
		{
			Mock<ILogManager> mockLogManager = new Mock<ILogManager>();
			mockLogManager.Setup(x => x.Boot(It.IsAny<string>(), It.IsAny<object[]>()));
			
			const string dataPath = "D://Projects//SmaugCS//trunk//data";

            LuaMgr = new LuaManager(mockLogManager.Object.LogWrapper, dataPath);

			DatabaseManager dbMgr = new DatabaseManager(mockLogManager.Object);

            LuaGetFunctions.InitializeReferences(LuaMgr, dbMgr, dataPath);
            LuaCreateFunctions.InitializeReferences(LuaMgr, dbMgr, mockLogManager.Object);

			var luaProxy = LuaInterfaceProxy.Create();

			var luaFuncRepo = LuaHelper.RegisterFunctionTypes(null, typeof(LuaCreateFunctions));
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

		[Test]
		public void LuaCreateMudProgTest()
		{
			LuaMgr.Proxy.DoString(GetMudProgLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<MudProgData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Type, Is.EqualTo(MudProgTypes.Greet));
			Assert.That(result.ArgList, Is.EqualTo("100"));
			Assert.That(result.Script, Is.EqualTo("LMobCommand(\"cac\");LMobSay(\"Now your soul shall be mine!\");"));
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

		[Test]
		public void LuaCreateShopTest()
		{
			LuaMgr.Proxy.DoString(GetShopLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<ShopData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ShopType, Is.EqualTo(ShopTypes.Item));
			Assert.That(result.OpenHour, Is.EqualTo(7));
			Assert.That(result.CloseHour, Is.EqualTo(21));
			Assert.That(result.ItemTypes.Count, Is.EqualTo(2));
			Assert.That(result.ItemTypes.Contains(ItemTypes.Armor), Is.True);
			Assert.That(result.ItemTypes.Contains(ItemTypes.Weapon), Is.True);

			var itemShop = result.CastAs<ItemShopData>();
			Assert.That(itemShop, Is.Not.Null);
			Assert.That(itemShop.ProfitBuy, Is.EqualTo(130));
			Assert.That(itemShop.ProfitSell, Is.EqualTo(90));
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

		[Test]
		public void LuaCreateResetTest()
		{
			LuaMgr.Proxy.DoString(GetResetLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<ResetData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Type, Is.EqualTo(ResetTypes.Mob));
			Assert.That(result.Args[0], Is.EqualTo(100));
			Assert.That(result.Args[1], Is.EqualTo(104));
			Assert.That(result.Args[2], Is.EqualTo(1));
		}

		[Test]
		public void LuaCreateReset_AddReset_Test()
		{
			LuaMgr.Proxy.DoString(GetResetLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<ResetData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Resets.Count, Is.EqualTo(1));
			Assert.That(result.Resets[0].Type, Is.EqualTo(ResetTypes.Give));
			Assert.That(result.Resets[0].Args[0], Is.EqualTo(110));
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

		[Test]
		public void LuaCreateLiquid_Test()
		{
			LuaMgr.Proxy.DoString(GetLiquidLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<LiquidData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ID, Is.EqualTo(4));
			Assert.That(result.Name, Is.EqualTo("dark ale"));
			Assert.That(result.ShortDescription, Is.EqualTo("dark ale"));
			Assert.That(result.Color, Is.EqualTo("dark brown"));
			Assert.That(result.Type, Is.EqualTo(LiquidTypes.Alcohol));
			Assert.That(result.GetMod(ConditionTypes.Drunk), Is.EqualTo(1));
			Assert.That(result.GetMod(ConditionTypes.Full), Is.EqualTo(2));
			Assert.That(result.GetMod(ConditionTypes.Thirsty), Is.EqualTo(5));
			Assert.That(result.GetMod(ConditionTypes.Bloodthirsty), Is.EqualTo(7));
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

		[Test]
		public void LuaCreateHerb_Test()
		{
			LuaMgr.Proxy.DoString(GetHerbLuaScript());
			var result = LuaCreateFunctions.GetLastObject(typeof(HerbData)).CastAs<HerbData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Rounds, Is.EqualTo(12));
			Assert.That(result.MinimumPosition, Is.EqualTo(9));
			Assert.That(result.Slot, Is.EqualTo(1));
			Assert.That(result.HitVictimMessage, Is.EqualTo("You start to cough and choke!"));
			Assert.That(result.Target, Is.EqualTo(TargetTypes.OffensiveCharacter));
			Assert.That(result.SkillFunctionName, Is.EqualTo("spell_smaug"));
			Assert.That(result.DamageMessage, Is.EqualTo("smoke"));
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

		[Test]
		public void LuaCreateSkill_Test()
		{
			LuaMgr.Proxy.DoString(GetSkillLuaScript());
			var result = LuaCreateFunctions.GetLastObject(typeof(SkillData)).CastAs<SkillData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Flags, Is.EqualTo(33792));
			Assert.That(result.MinimumPosition, Is.EqualTo(9));
			Assert.That(result.Slot, Is.EqualTo(246));
			Assert.That(result.MinimumMana, Is.EqualTo(10));
			Assert.That(result.MinimumLevel, Is.EqualTo(51));
			Assert.That(result.Target, Is.EqualTo(TargetTypes.DefensiveCharacter));
			Assert.That(result.SkillFunctionName, Is.EqualTo("spell_smaug"));
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

		[Test]
		public void LuaCreateSmaugAffect_Test()
		{
			LuaMgr.Proxy.DoString(GetSmaugAffectLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<SmaugAffect>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Duration, Is.EqualTo("10"));
			Assert.That(result.Location, Is.EqualTo(13));
			Assert.That(result.Modifier, Is.EqualTo("-10"));
			Assert.That(result.Flags, Is.EqualTo(258));
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

		[Test]
		public void LuaCreateSpecialFunction_Test()
		{
			LuaMgr.Proxy.DoString(GetSpecFunLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<SpecialFunction>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Name, Is.EqualTo("spec_breath_any"));
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

		[Test]
		public void LuaCreateCommand_Test()
		{
			LuaMgr.Proxy.DoString(GetCommandLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<CommandData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Name, Is.EqualTo("auction"));
			Assert.That(result.FunctionName, Is.EqualTo("do_auction"));
			Assert.That(result.Position, Is.EqualTo(4));
			Assert.That(result.Level, Is.EqualTo(5));
			Assert.That(result.Log, Is.EqualTo(1));
			Assert.That(result.Flags, Is.EqualTo(1));
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

		[Test]
		public void LuaCreateSocial_Test()
		{
			LuaMgr.Proxy.DoString(GetSocialLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<SocialData>();

			Assert.That(result, Is.Not.Null); 
			Assert.That(result.Name, Is.EqualTo("accuse"));
			Assert.That(result.CharNoArg, Is.EqualTo("Accuse whom?"));
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

		[Test]
		public void LuaCreateSpellComponent_Test()
		{
			LuaMgr.Proxy.DoString(GetSpellCommponentLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<SpellComponent>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.RequiredType, Is.EqualTo(ComponentRequiredTypes.ItemVnum));
			Assert.That(result.RequiredData, Is.EqualTo("65"));
			Assert.That(result.OperatorType, Is.EqualTo(ComponentOperatorTypes.DecreaseValue0));
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
			sb.Append("class.this:SetPrimaryAttribute(\"strength\");");
			sb.Append("class.this:SetSecondaryAttribute(\"constitution\");");
			sb.Append("class.this:SetDeficientAttribute(\"charisma\");");
			sb.Append("class.this:SetType(\"warrior\");");
			sb.Append("class.this:AddSkill(\"aggressive style\", 20, 50);");
			return sb.ToString();
		}

		[Test]
		public void LuaCreateClassTest()
		{
			LuaMgr.Proxy.DoString(GetClassLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<ClassData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.SkillAdept, Is.EqualTo(85));
			Assert.That(result.ToHitArmorClass0, Is.EqualTo(18));
			Assert.That(result.ToHitArmorClass32, Is.EqualTo(6));
			Assert.That(result.MinimumHealthGain, Is.EqualTo(11));
			Assert.That(result.MaximumHealthGain, Is.EqualTo(15));
			Assert.That(result.BaseExperience, Is.EqualTo(1150));
		}

		[Test]
		public void LuaCreateClass_SetPrimaryAttribute_Test()
		{
			LuaMgr.Proxy.DoString(GetClassLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<ClassData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.PrimaryAttribute, Is.EqualTo(StatisticTypes.Strength));
		}

		[Test]
		public void LuaCreateClass_SetSecondaryAttribute_Test()
		{
			LuaMgr.Proxy.DoString(GetClassLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<ClassData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.SecondaryAttribute, Is.EqualTo(StatisticTypes.Constitution));
		}

		[Test]
		public void LuaCreateClass_SetDeficientAttribute_Test()
		{
			LuaMgr.Proxy.DoString(GetClassLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<ClassData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.DeficientAttribute, Is.EqualTo(StatisticTypes.Charisma));
		}

		[Test]
		public void LuaCreateClass_SetType_Test()
		{
			LuaMgr.Proxy.DoString(GetClassLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<ClassData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Type, Is.EqualTo(ClassTypes.Warrior));
		}

		[Test]
		public void LuaCreateClass_AddSkill_Test()
		{
			LuaMgr.Proxy.DoString(GetClassLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<ClassData>();

			Assert.That(result, Is.Not.Null);

			var skillAdept = result.Skills.FirstOrDefault(x => x.Skill.Equals("aggressive style"));

			Assert.That(skillAdept, Is.Not.Null);
			Assert.That(skillAdept.Level, Is.EqualTo(20));
			Assert.That(skillAdept.Adept, Is.EqualTo(50));
		}

		[Test]
		public void LuaCreateClass_AddSkill_Duplicate_Test()
		{
			var script = GetClassLuaScript();
			script += "class.this:AddSkill(\"aggressive style\", 20, 50);";

			Assert.Throws<InvalidDataException>(() => LuaMgr.Proxy.DoString(script));
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

		[Test]
		public void LuaCreateRaceTest()
		{
			LuaMgr.Proxy.DoString(GetRaceLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<RaceData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ClassRestriction, Is.EqualTo(512));
			Assert.That(result.Language, Is.EqualTo(1));
			Assert.That(result.MinimumAlignment, Is.EqualTo(-1000));
			Assert.That(result.MaximumAlignment, Is.EqualTo(1000));
			Assert.That(result.ExperienceMultiplier, Is.EqualTo(100));
			Assert.That(result.Height, Is.EqualTo(66));
			Assert.That(result.Weight, Is.EqualTo(150));
		}

		[Test]
		public void LuaCreateRace_AddWhereName_Test()
		{
			LuaMgr.Proxy.DoString(GetRaceLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<RaceData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.WhereNames[0], Is.EqualTo("<used as light>     "));
			Assert.That(result.WhereNames[1], Is.EqualTo("<worn on finger>    "));
		}

		[Test]
		public void LuaCreateRace_AddAffectedBy_Test()
		{
			LuaMgr.Proxy.DoString(GetRaceLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<RaceData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.AffectedBy.IsSet(AffectedByTypes.Infrared), Is.True);
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

		[Test]
		public void LuaCreateClanTest()
		{
            LuaMgr.Proxy.DoString(GetClanLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<ClanData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Class, Is.EqualTo(7));
			Assert.That(result.Board, Is.EqualTo(21433));
			Assert.That(result.RecallRoom, Is.EqualTo(21430));
			Assert.That(result.StoreRoom, Is.EqualTo(21434));
			Assert.That(result.ClanType, Is.EqualTo(ClanTypes.Guild));
		}
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

		[Test]
		public void LuaCreateDeityTest()
		{
			LuaMgr.Proxy.DoString(GetDeityLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<DeityData>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Name, Is.EqualTo("Maron"));
			Assert.That(result.Description, Is.EqualTo("This is a deity description"));
			Assert.That(result.Worshippers, Is.EqualTo(1673));
			Assert.That(result.GetFieldValue(DeityFieldTypes.Flee), Is.EqualTo(-2));
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

		[Test]
		public void LuaCreateLanguageTest()
		{
			LuaMgr.Proxy.DoString(GetLanguageLuaScript());
			var result = LuaCreateFunctions.LastObject.CastAs<Language.LanguageData>();

			Assert.That(result, Is.Not.Null); 
			Assert.That(result.Name, Is.EqualTo("elvish"));
			Assert.That(result.Alphabet, Is.EqualTo("iqqdakvtujfwghepcrslybszoz"));

			Assert.That(result.PreConversion.Count, Is.EqualTo(3));
			Assert.That(result.PreConversion[0], Is.Not.Null);
			Assert.That(result.PreConversion[0].OldValue, Is.EqualTo("the hour"));
			Assert.That(result.PreConversion[0].NewValue, Is.EqualTo("lumenn"));

			Assert.That(result.Conversion.Count, Is.EqualTo(2));
			Assert.That(result.Conversion[0], Is.Not.Null);
			Assert.That(result.Conversion[0].OldValue, Is.EqualTo("rr"));
			Assert.That(result.Conversion[0].NewValue, Is.EqualTo("r"));
		}
		#endregion

	}
}
