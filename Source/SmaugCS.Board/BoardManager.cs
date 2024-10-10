using SmaugCS.Logging;

namespace SmaugCS.Board;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class BoardManager(ILogManager logManager, IBoardRepository repository) : IBoardManager
{
  private readonly ILogManager _logManager = logManager;

  public IBoardRepository Repository { get; } = repository;

  public void Initialize()
  {
    Repository.Load();
  }
}