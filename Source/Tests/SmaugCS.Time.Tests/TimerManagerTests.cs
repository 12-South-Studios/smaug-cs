using System.Timers;
using FakeItEasy;
using Xunit;
using FluentAssertions;
using Library.Common;

namespace SmaugCS.Time.Tests;

public class TimerManagerTests
{
  private bool _callback;
  private static ITimerManager _mgr;

  public TimerManagerTests()
  {
    _mgr = new TimerManager();
  }

  private void Callback(object sender, ElapsedEventArgs elapsedEventArgs)
  {
    _callback = true;
  }

  [Fact]
  public void GetTimerTest()
  {
    int id = _mgr.AddTimer(200, Callback);

    CommonTimer timer = _mgr.GetTimer(id);

    timer.Id.Should().Be(id);
  }

  [Fact]
  public void DeleteTimerTest()
  {
    int id = _mgr.AddTimer(200, Callback);

    _mgr.DeleteTimer(id);

    CommonTimer timer = _mgr.GetTimer(id);

    timer.Should().BeNull();
  }
}