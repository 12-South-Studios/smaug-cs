using Realm.Library.Patterns.Repository;

namespace Realm.Library.Network
{
    /// <summary>
    /// Repository class used to track TcpUsers
    /// </summary>
    public class TcpUserRepository : Repository<string, ITcpUser>, ITcpUserRepository
    {
    }
}