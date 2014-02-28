using System;
using NUnit.Framework;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class PropertyTests
    {
        [TestCase(true, true)]
        [TestCase(false, false)]
        public void Persistable_Set_Test(bool setTo, bool expectedValue)
        {
            var prop = new Property("Test");

            prop.Persistable = setTo;

            Assert.That(prop.Persistable, Is.EqualTo(expectedValue));
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void Visible_Set_Test(bool setTo, bool expectedValue)
        {
            var prop = new Property("Test");

            prop.Visible = setTo;

            Assert.That(prop.Visible, Is.EqualTo(expectedValue));
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void Volatile_Set_Test(bool setTo, bool expectedValue)
        {
            var prop = new Property("Test");

            prop.Volatile = setTo;

            Assert.That(prop.Volatile, Is.EqualTo(expectedValue));
        }
    }
}
