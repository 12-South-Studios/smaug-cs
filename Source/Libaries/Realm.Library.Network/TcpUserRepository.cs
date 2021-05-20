using Realm.Standard.Patterns.Repository;

namespace Realm.Library.Network
{
    /// <inheritdoc />
    /// <summary>
    /// Repository class used to track TcpUsers
    /// </summary>
    public class TcpUserRepository : Repository<string, ITcpUser>, ITcpUserRepository
    {
    }
}