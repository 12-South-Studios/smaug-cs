using System;

namespace Realm.Library.Pattern.Tests
{
    /*[TestClass]
    public class FactoryTest
    {
        internal class TestObject
        {}
        internal class DerivedTestObject : TestObject
        {}

        internal class TestFactory : Factory<Type, TestObject>
        {
        }

        private static TestFactory GetFactory()
        {
            var factory = new TestFactory();
            factory.Register<DerivedTestObject>(typeof(DerivedTestObject));
            return factory;
        }

        [TestMethod]
        public void Factory_Create_NotNull_Test()
        {
            var factory = GetFactory();
            var obj = factory.Create(typeof(DerivedTestObject));

            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void Factory_Create_Null_Test()
        {
            var factory = new TestFactory();
            var obj = factory.Create(typeof(DerivedTestObject));

            Assert.IsNull(obj);
        }

        [TestMethod]
        public void Factory_Create_ObjectDerives_Test()
        {
            var factory = GetFactory();
            var obj = factory.Create(typeof(DerivedTestObject));

            Assert.IsInstanceOfType(obj, typeof(TestObject));
        }
    }*/
}
