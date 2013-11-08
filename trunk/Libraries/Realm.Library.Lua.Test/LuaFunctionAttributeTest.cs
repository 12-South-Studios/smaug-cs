using NUnit.Framework;

namespace Realm.Library.Lua.Test
{
    [TestFixture]
    public class LuaFunctionAttributeTest
    {
        [Test]
        public void LuaFunctionAttribute_Constructor1_Test()
        {
            const string name = "test";
            const string desc = "description";

            var actual = new LuaFunctionAttribute(name, desc);
            
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Name, Is.EqualTo(name));
            Assert.That(actual.Description, Is.EqualTo(desc));
        }

        [Test]
        public void LuaFunctionAttribute_Constructor2_Test()
        {
            const string name = "test";
            const string desc = "description";
            string[] parameters = new[] {"test1", "test2", "test3"};

            var actual = new LuaFunctionAttribute(name, desc, parameters);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Name, Is.EqualTo(name));
            Assert.That(actual.Description, Is.EqualTo(desc));
            Assert.That(actual.Parameters, Is.EqualTo(parameters));
        }
    }
}
