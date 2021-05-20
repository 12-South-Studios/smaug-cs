using System;
using FluentAssertions;
using Realm.Library.Common.Extensions;
using Xunit;
using TypeExtensions = Realm.Library.Common.Extensions.TypeExtensions;

namespace Realm.Library.Common.Test.Extensions
{
    public static class TypeExtensionsTest
    {
        private class HelperObject
        {
            public HelperObject() { }

            public HelperObject(string arg1)
            {
                Arg1 = arg1;
            }

            public string Arg1 { get; set; }
        }

        [Fact]
        public static void InstantiateNullTest()
        {
            Action act = () => TypeExtensions.Instantiate<HelperObject>(null, null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void InstantiateTest()
        {
            var type = typeof(HelperObject);

            var obj = type.Instantiate<HelperObject>();

            obj.Should().NotBeNull();
            obj.GetType().Should().Be(type);
        }

        [Fact]
        public static void InstantiateWithArgumentsTest()
        {
            var type = typeof(HelperObject);
            const string arg1 = "Test Argument";

            var obj = type.Instantiate<HelperObject>(arg1);

            obj.Should().NotBeNull();
            obj.Arg1.Should().Be(arg1);
        }
    }
}
