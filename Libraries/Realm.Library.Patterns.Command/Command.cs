using System.Collections.Generic;

namespace Realm.Library.Patterns.Command
{
    /// <summary>
    /// Abstract Command class
    /// </summary>
    public abstract class Command : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class
        /// </summary>
        /// <param name="name">Name of the command</param>
        /// <param name="action">Log action to take when executed</param>
        protected Command(string name, LogAction action)
        {
            Name = name;
            Action = action;
        }

        /// <summary>
        /// Gets the name of the command
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of logging for this command
        /// </summary>
        public LogAction Action { get; private set; }

        /// <summary>
        /// Executes the command using the passed parameters
        /// </summary>
        public abstract void Execute(IDictionary<string, object> args);

        /// <summary>
        /// Executes the command
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Determines if the command can be executed
        /// </summary>
        public virtual bool CanExecute(object obj)
        {
            return true;
        }
    }
}