using FluentAssertions;
using Patterns.Decorator;
using Xunit;

namespace Patterns.Tests;

public class DecoratorTests
{
  private class ConcreteComponent : IComponent;

  private class ConcreteDecorator(IComponent component) : global::Patterns.Decorator.Decorator(component);

  [Fact]
  public void DecoratorPatternTest()
  {
    ConcreteComponent concrete = new();
    ConcreteDecorator decorator = new(concrete);
    concrete.Should().Be(decorator.Component);
  }
}