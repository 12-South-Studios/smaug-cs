using Realm.Standard.Patterns.Repository;

namespace Realm.Library.Network.Tcp
{
    public class TcpUserRepository : Repository<string, TcpUser>, IUserRepository<string, TcpUser>
    {
    }
}