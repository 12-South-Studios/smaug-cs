using Realm.Library.Patterns.Repository;

namespace Realm.Library.Patterns.Command
{
    /// <summary>
    /// Declares the interface for a command repository
    /// </summary>
    public interface ICommandRepository : IRepository<string, ICommand>
    {
    }
}