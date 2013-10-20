using System;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
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

        [Test]
        public static void InstantiateNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => TypeExtensions.Instantiate<HelperObject>(null, null),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        public static void InstantiateTest()
        {
            var type = typeof(HelperObject);

            var obj = type.Instantiate<HelperObject>();

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.GetType(), Is.EqualTo(type));
        }

        [Test]
        public static void InstantiateWithArgumentsTest()
        {
            var type = typeof(HelperObject);
            const string arg1 = "Test Argument";

            var obj = type.Instantiate<HelperObject>(arg1);

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.Arg1, Is.EqualTo(arg1));
        }
    }
}
