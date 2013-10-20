using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
    public class ObjectExtensionTest
    {
        [Test]
        public void CastAsTest()
        {
            const string value = "1";

            Assert.That(value.CastAs<int>(), Is.EqualTo(1));
        }

        [TestCase("1", 1)]
        [TestCase("one", 5)]
        public void TryCastAsTest(string value, int expected)
        {
            Assert.That(value.TryCastAs(expected), Is.EqualTo(expected));
        }

        [Test]
        public void ToNullableTest()
        {
            const int intValue = 5;
            int? nullableValue = 5;

            var actual = intValue.ToNullable<int>();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.EqualTo(nullableValue));
            Assert.That(actual.GetValueOrDefault(1), Is.EqualTo(intValue));
        }

        [Test]
        public void OrElseTest()
        {
            var obj = new object();
            var result = obj.OrElse(null);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(obj));
        }

        [Test]
        public void IsNullOrDbNull_IsNull_Test()
        {
            Assert.That(ObjectExtensions.IsNullOrDBNull<object>(null), Is.True);
        }

        [Test]
        public void IsNullOrDbNull_IsDbNull_Test()
        {
            Assert.That(ObjectExtensions.IsNullOrDBNull<object>(DBNull.Value), Is.True);
        }

        [Test]
        public void IsEmptyTest()
        {
            var list = new List<int>();

            Assert.That(list.IsEmpty(), Is.True);
        }
    }
}
