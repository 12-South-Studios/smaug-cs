using System.Text;
using NUnit.Framework;
using Realm.Library.Lua;
using SmaugCS.Database;
using SmaugCS.Managers;

namespace SmaugCS.Tests.Repositories
{
    [TestFixture]
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
            sb.Append("area.this:SetRanges(15, 35, 0, 60);");
            sb.Append("area.this:SetEconomy(0, 2064393);");
            sb.Append(
                "area.this.ResetMessage = \"The astral field glitters with a thousand points of light for a moment...\";");
            return sb.ToString();
        }

        [Test]
        public void LuaCreateAreaTest()
        {
            var luaProxy = new LuaInterfaceProxy();

            var luaFuncRepo = LuaHelper.RegisterFunctionTypes(null, typeof(AreaRepository));
            luaProxy.RegisterFunctions(luaFuncRepo);

            LuaManager.Instance.InitProxy(luaProxy);

            var result = AreaRepository.LuaProcessArea(GetAreaLuaScript());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ID, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("The Astral"));
            Assert.That(result.Author, Is.EqualTo("Andi"));
            Assert.That(result.LowEconomy, Is.EqualTo(2064393));
            Assert.That(result.HighHardRange, Is.EqualTo(60));
            Assert.That(result.ResetMessage, Is.EqualTo("The astral field glitters with a thousand points of light for a moment..."));
        }
    }
}
