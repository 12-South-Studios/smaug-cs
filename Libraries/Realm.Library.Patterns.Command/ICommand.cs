using System.Collections.Generic;

namespace Realm.Library.Patterns.Command
{
    /// <summary>
    /// Declares the contract for a command
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets the name of the command
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the logging state of the command
        /// </summary>
        LogAction Action { get; }

        /// <summary>
        /// Executes the command using the given arguments
        /// </summary>
        /// <param name="args"></param>
        void Execute(IDictionary<string, object> args);

        /// <summary>
        /// Executes the command without arguments
        /// </summary>
        void Execute();

        /// <summary>
        /// Gets whether or not the object can execute the command
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool CanExecute(object obj);
    }
}