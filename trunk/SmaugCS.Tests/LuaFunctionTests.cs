using System.Text;
using NUnit.Framework;
using Realm.Library.Common;
using Realm.Library.Lua;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Shops;
using SmaugCS.Managers;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class LuaFunctionTests
    {
        #region Script Functions
        private static string GetMudProgLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newProg = LCreateMudProg(\"greet_prog\");");
            sb.Append("mprog.this = newProg;");
            sb.Append("mprog.this.ArgList = \"100\";");
            sb.Append("mprog.this.Script = [[LMobCommand(\"cac\");LMobSay(\"Now your soul shall be mine!\");]];");
            return sb.ToString();
        }

        private static string GetShopLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newShop = LCreateShop(130, 90, 7, 21);");
            sb.Append("shop.this = newShop;");
            sb.Append("shop.this:AddItemType(\"armor\");");
            sb.Append("shop.this:AddItemType(\"weapon\");");
            return sb.ToString();
        }

        private static string GetResetLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newReset = LCreateReset(\"mob\", 0, 100, 104, 1);");
            sb.Append("reset.this = newReset;");
            sb.Append("reset.this:AddReset(\"give\", 0, 110, 0, 0);");
            return sb.ToString();
        }

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

        private static string GetHerbLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newHerb = LCreateSkill(\"black gwyvel\", \"herb\");");
            sb.Append("herb.this = newHerb;");
            sb.Append("herb.this.Rounds = 12;");
            sb.Append("herb.this.DamageMessage = \"smoke\";");
            sb.Append("herb.this:SetTargetByValue(1);");
            sb.Append("herb.this.MinimumPosition = 7;");
            sb.Append("herb.this.Slot = 1;");
            sb.Append("herb.this.HitVictimMessage = \"You start to cough and choke!\";");
            sb.Append("LSetCode(herb.this, \"spell_smaug\");");
            sb.Append("newAffect = LCreateSmaugAffect(\"\", 13, \"-10\", 0);");
            sb.Append("affect.this = newAffect;");
            sb.Append("herb.this:AddAffect(affect.this);");
            return sb.ToString();
        }

        private static string GetSkillLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newSkill = LCreateSkill(\"Wrath of Dominus\", \"spell\");");
            sb.Append("skill.this = newSkill;");
            sb.Append("skill.this:SetFlags(\"secretskill noscribe\");");
            sb.Append("skill.this:SetTargetByValue(2);");
            sb.Append("skill.this.MinimumPosition = 109;");
            sb.Append("skill.this.Slot = 246;");
            sb.Append("skill.this.MinimumMana = 10;");
            sb.Append("skill.this.MinimumLevel = 51;");
            sb.Append("LSetCode(skill.this, \"spell_smaug\");");
            sb.Append("newAffect = LCreateSmaugAffect(\"10\", 13, \"-75\", -1);");
            sb.Append("affect.this = newAffect;");
            sb.Append("skill.this:AddAffect(affect.this);");
            return sb.ToString();
        }

        private static string GetSmaugAffectLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newAffect = LCreateSmaugAffect(\"10\", 13, \"-10\", 258)");
            sb.Append("affect.this = newAffect;");
            return sb.ToString();
        }

        private static string GetSpecFunLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newSpecFun = LCreateSpecFun(\"spec_breath_any\");");
            sb.Append("specfun.this = newSpecFun;");
            return sb.ToString();
        }

        private static string GetCommandLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newCommand = LCreateCommand(\"auction\", \"do_auction\", 104, 5, 1, 1);");
            sb.Append("command.this = newCommand;");
            return sb.ToString();
        }

        private static string GetSocialLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newSocial = LCreateSocial(\"accuse\")");
            sb.Append("social.this = newSocial;");
            sb.Append("social.this.CharNoArg = \"Accuse whom?\";");
            return sb.ToString();
        }

        private static string GetSpellCommponentLuaScript()
        {
            var sb = new StringBuilder();
            sb.Append("newComponent = LCreateSpellComponent(\"V\", \"65\", \"@\");");
            sb.Append("component.this = newComponent;");
            return sb.ToString();
        }
        #endregion

        [SetUp]
        public void OnSetup()
        {
            var luaProxy = new LuaInterfaceProxy();

            var luaFuncRepo = LuaHelper.RegisterFunctionTypes(null, typeof(LuaFunctions));
            luaProxy.RegisterFunctions(luaFuncRepo);

            LuaManager.Instance.InitProxy(luaProxy);
        }

        [Test]
        public void LuaCreateMudProgTest()
        {
            LuaManager.Instance.Proxy.DoString(GetMudProgLuaScript());
            var result = LuaFunctions.LastObject.CastAs<MudProgData>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Type, Is.EqualTo(MudProgTypes.Greet));
            Assert.That(result.ArgList, Is.EqualTo("100"));
            Assert.That(result.Script, Is.EqualTo("LMobCommand(\"cac\");LMobSay(\"Now your soul shall be mine!\");"));
        }

        [Test]
        public void LuaCreateShopTest()
        {
            LuaManager.Instance.Proxy.DoString(GetShopLuaScript());
            var result = LuaFunctions.LastObject.CastAs<ShopData>();

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

        [Test]
        public void LuaCreateResetTest()
        {
            LuaManager.Instance.Proxy.DoString(GetResetLuaScript());
            var result = LuaFunctions.LastObject.CastAs<ResetData>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Type, Is.EqualTo(ResetTypes.Mob));
            Assert.That(result.Args[0], Is.EqualTo(100));
            Assert.That(result.Args[1], Is.EqualTo(104));
            Assert.That(result.Args[2], Is.EqualTo(1));
        }

        [Test]
        public void LuaCreateReset_AddReset_Test()
        {
            LuaManager.Instance.Proxy.DoString(GetResetLuaScript());
            var result = LuaFunctions.LastObject.CastAs<ResetData>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Resets.Count, Is.EqualTo(1));
            Assert.That(result.Resets[0].Type, Is.EqualTo(ResetTypes.Give));
            Assert.That(result.Resets[0].Args[0], Is.EqualTo(110));
        }

        [Test]
        public void LuaCreateLiquid_Test()
        {
            LuaManager.Instance.Proxy.DoString(GetLiquidLuaScript());
            var result = LuaFunctions.LastObject.CastAs<LiquidData>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Vnum, Is.EqualTo(4));
            Assert.That(result.Name, Is.EqualTo("dark ale"));
            Assert.That(result.ShortDescription, Is.EqualTo("dark ale"));
            Assert.That(result.Color, Is.EqualTo("dark brown"));
            Assert.That(result.Type, Is.EqualTo(LiquidTypes.Alcohol));
            Assert.That(result.GetMod(ConditionTypes.Drunk), Is.EqualTo(1));
            Assert.That(result.GetMod(ConditionTypes.Full), Is.EqualTo(2));
            Assert.That(result.GetMod(ConditionTypes.Thirsty), Is.EqualTo(5));
            Assert.That(result.GetMod(ConditionTypes.Bloodthirsty), Is.EqualTo(7));
        }

        [Test]
        public void LuaCreateHerb_Test()
        {
            LuaManager.Instance.Proxy.DoString(GetHerbLuaScript());
            var result = LuaFunctions.GetLastObject(typeof(SkillData)).CastAs<SkillData>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Rounds, Is.EqualTo(12));
            Assert.That(result.MinimumPosition, Is.EqualTo(9));
            Assert.That(result.Slot, Is.EqualTo(1));
            Assert.That(result.HitVictimMessage, Is.EqualTo("You start to cough and choke!"));
            Assert.That(result.Target, Is.EqualTo(TargetTypes.OffensiveCharacter));
            Assert.That(result.DamageMessage, Is.EqualTo("smoke"));
        }

        [Test]
        public void LuaCreateHerb_LuaSetCode_Test()
        {
            LuaManager.Instance.Proxy.DoString(GetHerbLuaScript());
            var result = LuaFunctions.GetLastObject(typeof(SkillData)).CastAs<SkillData>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.SpellFunctionName, Is.EqualTo("spell_smaug"));
            Assert.That(result.SpellFunction, Is.Not.Null);
            Assert.That(result.SpellFunction.Value, Is.Not.Null);
        }

        [Test]
        public void LuaCreateSkill_Test()
        {
            LuaManager.Instance.Proxy.DoString(GetSkillLuaScript());
            var result = LuaFunctions.GetLastObject(typeof(SkillData)).CastAs<SkillData>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Flags, Is.EqualTo(33792));
            Assert.That(result.MinimumPosition, Is.EqualTo(9));
            Assert.That(result.Slot, Is.EqualTo(246));
            Assert.That(result.MinimumMana, Is.EqualTo(10));
            Assert.That(result.MinimumLevel, Is.EqualTo(51));
            Assert.That(result.Target, Is.EqualTo(TargetTypes.DefensiveCharacter));
        }

        [Test]
        public void LuaCreateSmaugAffect_Test()
        {
            LuaManager.Instance.Proxy.DoString(GetSmaugAffectLuaScript());
            var result = LuaFunctions.LastObject.CastAs<SmaugAffect>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Duration, Is.EqualTo("10"));
            Assert.That(result.Location, Is.EqualTo(13));
            Assert.That(result.Modifier, Is.EqualTo("-10"));
            Assert.That(result.Flags, Is.EqualTo(258));
        }
        
        [Test]
        public void LuaCreateSpecialFunction_Test()
        {
            LuaManager.Instance.Proxy.DoString(GetSpecFunLuaScript());
            var result = LuaFunctions.LastObject.CastAs<SpecialFunction>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("spec_breath_any"));
            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        public void LuaCreateCommand_Test()
        {
            LuaManager.Instance.Proxy.DoString(GetCommandLuaScript());
            var result = LuaFunctions.LastObject.CastAs<CommandData>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("auction"));
            Assert.That(result.FunctionName, Is.EqualTo("do_auction"));
            Assert.That(result.Position, Is.EqualTo(4));
            Assert.That(result.Level, Is.EqualTo(5));
            Assert.That(result.Log, Is.EqualTo(1));
            Assert.That(result.Flags, Is.EqualTo(1));
            Assert.That(result.DoFunction, Is.Not.Null);
            Assert.That(result.DoFunction.Value, Is.Not.Null);
        }

        [Test]
        public void LuaCreateSocial_Test()
        {
            LuaManager.Instance.Proxy.DoString(GetSocialLuaScript());
            var result = LuaFunctions.LastObject.CastAs<SocialData>();

            Assert.That(result, Is.Not.Null); 
            Assert.That(result.Name, Is.EqualTo("accuse"));
            Assert.That(result.CharNoArg, Is.EqualTo("Accuse whom?"));
        }

        [Test]
        public void LuaCreateSpellComponent_Test()
        {
            LuaManager.Instance.Proxy.DoString(GetSpellCommponentLuaScript());
            var result = LuaFunctions.LastObject.CastAs<SpellComponent>();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RequiredType, Is.EqualTo(ComponentRequiredTypes.ItemVnum));
            Assert.That(result.RequiredData, Is.EqualTo("65"));
            Assert.That(result.OperatorType, Is.EqualTo(ComponentOperatorTypes.DecreaseValue0));
        }
    }
}
