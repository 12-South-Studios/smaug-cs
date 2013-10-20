using Realm.Library.Patterns.Repository;

namespace Realm.Library.Patterns.Command
{
    /// <summary>
    /// Defines a command repository class
    /// </summary>
    public class CommandRepository : Repository<string, ICommand>, ICommandRepository
    {
    }
}