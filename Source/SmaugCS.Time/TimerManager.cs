using Library.Common;
using System.Collections.Concurrent;
using System.Linq;
using System.Timers;

namespace SmaugCS.Time;

public sealed class TimerManager : ITimerManager
{
  private static int _idSpace = 1;
  private static int GetNextId => _idSpace++;

  private readonly ConcurrentDictionary<int, CommonTimer> _timerTable = new();

  ~TimerManager()
  {
    _timerTable.Values.ToList().ForEach(x => x.Dispose());
  }

  public int AddTimer(double duration, ElapsedEventHandler callback)
  {
    int newId = GetNextId;

    CommonTimer newTimer = new(newId) { Interval = duration };
    newTimer.Elapsed += callback;

    _timerTable.GetOrAdd(newId, newTimer);
    newTimer.Start();
    return newId;
  }

  public CommonTimer GetTimer(int timerId)
  {
    _timerTable.TryGetValue(timerId, out CommonTimer timer);
    return timer;
  }

  public void DeleteTimer(int timerId)
  {
    _timerTable.TryRemove(timerId, out CommonTimer timer);
    timer?.Dispose();
  }
}