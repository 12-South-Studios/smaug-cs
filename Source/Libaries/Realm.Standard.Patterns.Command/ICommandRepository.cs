using Realm.Standard.Patterns.Repository;

namespace Realm.Standard.Patterns.Command
{
    public interface ICommandRepository : IRepository<string, ICommand>
    {
    }
}
