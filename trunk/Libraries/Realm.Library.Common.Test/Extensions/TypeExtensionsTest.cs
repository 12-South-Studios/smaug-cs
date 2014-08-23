using System;
using NUnit.Framework;
using Realm.Library.Common.Test.Fakes;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
    public static class TypeExtensionsTest
    {
        [Test]
        [Category("Extension Tests")]
        public static void InstantiateNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => TypeExtensions.Instantiate<HelperObject>(null, null),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        [Category("Extension Tests")]
        public static void InstantiateTest()
        {
            var type = typeof(HelperObject);

            var obj = type.Instantiate<HelperObject>();

            Assert.That(obj, Is.Not.Null);
            Assert.That(obj.GetType(), Is.EqualTo(type));
        }

        [Test]
        [Category("Extension Tests")]
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
