using FluentAssertions;
using Realm.Standard.Patterns.Decorator;
using Xunit;

namespace Realm.Standard.Patterns.Tests
{
    public class DecoratorTests
    {
        private class ConcreteComponent : IComponent
        { }

        private class ConcreteDecorator : Decorator.Decorator
        {
            public ConcreteDecorator(IComponent component)
                : base(component)
            {
            }
        }

        [Fact]
        public void DecoratorPatternTest()
        {
            var concrete = new ConcreteComponent();
            var decorator = new ConcreteDecorator(concrete);
            concrete.Should().Be(decorator.Component);
        }
    }
}
