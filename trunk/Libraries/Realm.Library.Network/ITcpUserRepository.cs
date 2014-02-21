using Realm.Library.Patterns.Repository;

namespace Realm.Library.Network
{
    /// <summary>
    /// Defines the contract for a TcpUserRepository
    /// </summary>
    public interface ITcpUserRepository : IRepository<string, ITcpUser>
    {
    }
}