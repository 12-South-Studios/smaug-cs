using System.Collections.Generic;

namespace Realm.Standard.Patterns.Command
{
    public interface ICommand
    {
        string Name { get; }

        void Execute(IDictionary<string, object> args = null);

        bool CanExecute(object obj);
    }
}
