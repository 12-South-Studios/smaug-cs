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
    }
}
