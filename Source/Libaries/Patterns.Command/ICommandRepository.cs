using Patterns.Repository;

namespace Patterns.Command;

/// <summary>
/// 
/// </summary>
public interface ICommandRepository : IRepository<string, ICommand>;