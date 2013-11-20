using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class EnumerationFunctionTests
    {
        private enum Test
        {
            [Enum("Test One")]
            Test1 = 1,

            [Enum("Test Two")]
            Test2
        }

        [TestCase("Test One", Test.Test1)]
        [TestCase("Test Two", Test.Test2)]
        public void GetEnumByNameTest(string name, Enum expectedVal)
        {
            Assert.That(EnumerationFunctions.GetEnumByName<Test>(name), Is.EqualTo(expectedVal));
        }

        [Test]
        public void GetValuesTest()
        {
            IEnumerable<Test> list = EnumerationFunctions.GetAllEnumValues<Test>();

            Assert.That(list, Is.Not.Null);

            List<Test> enumList = list.ToList();
            Assert.That(enumList, Has.Member(Test.Test1));
            Assert.That(enumList, Has.Member(Test.Test2));
        }

        [Test]
        public void MaxTest()
        {
            Assert.That(EnumerationFunctions.MaximumEnumValue<Test>(), Is.EqualTo(2));
        }

        [Test]
        public void MinTest()
        {
            Assert.That(EnumerationFunctions.MinimumEnumValue<Test>(), Is.EqualTo(1));
        }
    }
}
