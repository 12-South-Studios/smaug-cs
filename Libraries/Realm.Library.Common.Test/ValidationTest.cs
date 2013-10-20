using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class ValidationTest
    {
        [TestCase(null, "null", ExpectedException = typeof(ArgumentNullException))]
        [TestCase(25, "number")]
        public void IsNotNullTest(object obj, string param)
        {
            Validation.IsNotNull(obj, param);
        }

        [TestCase(25, typeof(int))]
        [TestCase("test", typeof(string))]
        [TestCase(null, typeof(int), ExpectedException = typeof(ArgumentNullException))]
        [TestCase("test", (Type)null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase(25, typeof(bool), ExpectedException = typeof(ArgumentException))]
        public void IsInstanceOfTest(object obj, Type type)
        {
            Validation.IsInstanceOfType(obj, type);
        }

        [TestCase("", "null", ExpectedException = typeof(ArgumentNullException))]
        [TestCase("test", "number")]
        public void IsNotNullOrEmptyTest(string obj, string param)
        {
            Validation.IsNotNullOrEmpty(obj, param);
        }

        [TestCase(null, "null", ExpectedException = typeof(ArgumentNullException))]
        [TestCase(new object[] { }, "object", ExpectedException = typeof(ArgumentException))]
        [TestCase(new object[] { 25, 15 }, "value")]
        public void IsNotEmpty(ICollection value, string param)
        {
            Validation.IsNotEmpty(value, param);
        }

        [Test]
        public void IsNotEmptyByTypeTest()
        {
            ICollection<string> list = new Collection<string>();
            list.Add("Test1");
            list.Add("Test2");

            Assert.DoesNotThrow(() => Validation.IsNotEmpty(list, "list"));
        }

        [TestCase(true, "test")]
        [TestCase(false, "test", ExpectedException = typeof(ArgumentException))]
        public void Validate(bool test, string message)
        {
            Validation.Validate(test, message);
        }

        [TestCase(true, "test")]
        [TestCase(false, "test", ExpectedException = typeof(InconclusiveException))]
        public void ValidateCustomException(bool test, string message)
        {
            Validation.Validate<InconclusiveException>(test, message);
        }

        [Test]
        public void ValidateAction()
        {
            var callback = false;

            Validation.Validate(() => { callback = true; });

            Assert.That(callback, Is.True);
        }
    }
}
