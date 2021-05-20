using System.Collections.Generic;

namespace Realm.Standard.Patterns.Command
{
    public abstract class Command : ICommand
    {
        protected Command(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public abstract void Execute(IDictionary<string, object> args = null);

        public virtual bool CanExecute(object obj)
        {
            return true;
        }
    }
}
