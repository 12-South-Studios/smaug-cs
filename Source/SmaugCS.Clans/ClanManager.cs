using SmaugCS.Logging;

namespace SmaugCS.Clans;

public sealed class ClanManager(ILogManager logManager, IClanRepository repository) : IClanManager
{
  private readonly ILogManager _logManager = logManager;

  public IClanRepository Repository { get; private set; } = repository;

  public void Initialize()
  {
  }
}