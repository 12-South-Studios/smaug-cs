using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Realm.Standard.Patterns.Tests
{
    public class CommandTests
    {
        private class FakeCommand : Command.Command
        {
            public FakeCommand(string name) : base(name) { }

            #region Overrides of Command

            public override void Execute(IDictionary<string, object> args)
            {
                // do nothing
            }

            #endregion
        }

        [Fact]
        public void Command_Constructor_Test()
        {
            const string name = "Test";

            var actual = new FakeCommand(name);
            actual.Name.Should().Be(name);
        }

        [Fact]
        public void Command_CanExecute_Test()
        {
            const string name = "Test";

            var command = new FakeCommand(name);
            var result = command.CanExecute(new object());
            result.Should().BeTrue();
        }
    }
}
