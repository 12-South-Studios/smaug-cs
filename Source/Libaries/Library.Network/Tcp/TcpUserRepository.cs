using Patterns.Repository;

namespace Library.Network.Tcp;

public class TcpUserRepository : Repository<string, TcpUser>, IUserRepository<string, TcpUser>;