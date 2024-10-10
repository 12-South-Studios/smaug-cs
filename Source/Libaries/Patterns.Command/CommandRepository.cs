using Patterns.Repository;

namespace Patterns.Command;

public class CommandRepository : Repository<string, ICommand>, ICommandRepository;