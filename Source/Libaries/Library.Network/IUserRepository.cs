using Patterns.Repository;

namespace Library.Network;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TK"></typeparam>
public interface IUserRepository<T, TK> : IRepository<T, TK> where TK : INetworkUser;