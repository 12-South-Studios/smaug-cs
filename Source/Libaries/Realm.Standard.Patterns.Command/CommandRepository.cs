using Realm.Standard.Patterns.Repository;

namespace Realm.Standard.Patterns.Command
{
    public class CommandRepository : Repository<string, ICommand>, ICommandRepository
    {
    }
}
