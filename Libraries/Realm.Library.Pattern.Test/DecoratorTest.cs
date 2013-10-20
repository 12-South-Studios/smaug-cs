using NUnit.Framework;
using Realm.Library.Patterns.Decorator;

namespace Realm.Library.Pattern.Test
{
    [TestFixture]
    public class DecoratorTest
    {
        private class ConcreteComponent : Patterns.Decorator.IComponent
        { }

        private class ConcreteDecorator : Decorator
        {
            public ConcreteDecorator(Patterns.Decorator.IComponent component)
                : base(component)
            {
            }
        }

        [Test]
        public void DecoratorPatternTest()
        {
            var concrete = new ConcreteComponent();
            var decorator = new ConcreteDecorator(concrete);

            Assert.That(concrete, Is.EqualTo(decorator.Component));
        }
    }
}
