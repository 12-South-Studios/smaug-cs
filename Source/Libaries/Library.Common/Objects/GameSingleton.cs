using Library.Common.Data;
using Library.Common.Events;
using Library.Common.Logging;
using Patterns.Singleton;

namespace Library.Common.Objects;

/// <summary>
/// 
/// </summary>
public abstract class GameSingleton : Singleton, IGameSingleton
{
  /// <summary>
  /// 
  /// </summary>
  /// <param name="args"></param>
  public virtual void Instance_OnGameInitialize(RealmEventArgs args)
  {
    BooleanSet booleanSet = args.GetValue("BooleanSet") as BooleanSet;
    if (booleanSet.IsNull()) return;

    booleanSet?.CompleteItem(GetType().Name);

    DictionaryAtom initAtom = args.GetValue("InitAtom") as DictionaryAtom;
    LogWrapper logger = initAtom?.GetObject("Logger").CastAs<LogWrapper>();
    logger?.Info("Manager {0} initialized.", GetType());
  }
}