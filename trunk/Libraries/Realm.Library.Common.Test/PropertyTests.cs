using NUnit.Framework;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class PropertyTests
    {
        [TestCase(true, true)]
        [TestCase(false, false)]
        [Category("Object Tests")]
        public void Persistable_Set_Test(bool setTo, bool expectedValue)
        {
            var prop = new Property("Test") {Persistable = setTo};

            Assert.That(prop.Persistable, Is.EqualTo(expectedValue));
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        [Category("Object Tests")]
        public void Visible_Set_Test(bool setTo, bool expectedValue)
        {
            var prop = new Property("Test") {Visible = setTo};

            Assert.That(prop.Visible, Is.EqualTo(expectedValue));
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        [Category("Object Tests")]
        public void Volatile_Set_Test(bool setTo, bool expectedValue)
        {
            var prop = new Property("Test") {Volatile = setTo};

            Assert.That(prop.Volatile, Is.EqualTo(expectedValue));
        }
    }
}
