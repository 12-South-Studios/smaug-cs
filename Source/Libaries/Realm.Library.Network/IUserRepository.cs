using Realm.Standard.Patterns.Repository;

namespace Realm.Library.Network
{
    public interface IUserRepository<T, TK> : IRepository<T, TK> where TK : INetworkUser
    {
    }
}
