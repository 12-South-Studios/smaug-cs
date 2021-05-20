using FluentAssertions;
using Xunit;

namespace Realm.Library.Lua.Test
{
    public class LuaFunctionAttributeTest
    {
        [Fact]
        public void LuaFunctionAttribute_Constructor1_Test()
        {
            const string name = "test";
            const string desc = "description";

            var actual = new LuaFunctionAttribute(name, desc);

            actual.Should().NotBeNull();
            actual.Name.Should().Be(name);
            actual.Description.Should().Be(desc);
        }

        [Fact]
        public void LuaFunctionAttribute_Constructor2_Test()
        {
            const string name = "test";
            const string desc = "description";
            string[] parameters = new[] {"test1", "test2", "test3"};

            var actual = new LuaFunctionAttribute(name, desc, parameters);

            actual.Should().NotBeNull();
            actual.Name.Should().Be(name);
            actual.Description.Should().Be(desc);
            actual.Parameters.Should().BeEquivalentTo(parameters);
        }
    }
}
