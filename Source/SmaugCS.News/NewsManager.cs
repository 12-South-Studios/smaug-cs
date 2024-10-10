using SmaugCS.Logging;

namespace SmaugCS.News;

public sealed class NewsManager(ILogManager logManager, INewsRepository repository) : INewsManager
{
  private readonly ILogManager _logManager = logManager;

  public INewsRepository Repository { get; private set; } = repository;

  public void Initialize()
  {
  }
}