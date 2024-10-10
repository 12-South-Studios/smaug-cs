using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Patterns.Tests;

public class CommandTests
{
  private class FakeCommand(string name) : global::Patterns.Command.Command(name)
  {
    public override void Execute(IDictionary<string, object> args)
    {
      // do nothing
    }
  }

  [Fact]
  public void Command_Constructor_Test()
  {
    const string name = "Test";

    FakeCommand actual = new(name);
    actual.Name.Should().Be(name);
  }

  [Fact]
  public void Command_CanExecute_Test()
  {
    const string name = "Test";

    FakeCommand command = new(name);
    bool result = command.CanExecute(new object());
    result.Should().BeTrue();
  }
}