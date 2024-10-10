using System;
using FluentAssertions;
using Library.Common.Extensions;
using Xunit;
using TypeExtensions = Library.Common.Extensions.TypeExtensions;

namespace Library.Common.Tests.Extensions;

public static class TypeExtensionsTest
{
  private class HelperObject
  {
    public HelperObject()
    {
    }

    public HelperObject(string arg1)
    {
      Arg1 = arg1;
    }

    public string Arg1 { get; set; }
  }

  [Fact]
  public static void InstantiateNullTest()
  {
    Action act = () => TypeExtensions.Instantiate<HelperObject>(null, null);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public static void InstantiateTest()
  {
    Type type = typeof(HelperObject);

    HelperObject obj = type.Instantiate<HelperObject>();

    obj.Should().NotBeNull();
    obj.GetType().Should().Be(type);
  }

  [Fact]
  public static void InstantiateWithArgumentsTest()
  {
    Type type = typeof(HelperObject);
    const string arg1 = "Test Argument";

    HelperObject obj = type.Instantiate<HelperObject>(arg1);

    obj.Should().NotBeNull();
    obj.Arg1.Should().Be(arg1);
  }
}