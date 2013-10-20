using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using Realm.Library.Patterns.Command;

namespace Realm.Library.Pattern.Test
{
    [TestFixture]
    public class CommandTest
    {
        private class FakeCommand : Command
        {
            public FakeCommand(string name, LogAction action) : base(name, action) {}

            #region Overrides of Command

            public override void Execute(IDictionary<string, object> args)
            {
                // do nothing
            }

            public override void Execute()
            {
                // do nothing
            }

            #endregion
        }

        [Test]
        public void Command_Constructor_Test()
        {
            const string name = "Test";
            const LogAction action = LogAction.Always;

            var actual = new FakeCommand(name, action);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Name, Is.EqualTo(name));
            Assert.That(actual.Action, Is.EqualTo(action));
        }

        [Test]
        public void Command_CanExecute_Test()
        {
            const string name = "Test";
            const LogAction action = LogAction.Always;

            var command = new FakeCommand(name, action);

            Assert.That(command.CanExecute(new object()), Is.True);
        }
    }
}
