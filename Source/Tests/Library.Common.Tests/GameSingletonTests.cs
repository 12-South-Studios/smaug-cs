using FakeItEasy;
using FluentAssertions;
using Library.Common.Data;
using Library.Common.Events;
using Library.Common.Logging;
using Library.Common.Objects;
using Xunit;

namespace Library.Common.Tests;

public class GameSingletonFacts
{
  public static bool _callback;

  private class FakeSingleton : GameSingleton;

  [Fact]
  public void OnGameInitialize_Fact()
  {
    _callback = false;

    LogWrapper logger = A.Fake<LogWrapper>();

    DictionaryAtom initAtom = new();
    initAtom.Add(new StringAtom("Logger"), new ObjectAtom(logger));

    BooleanSet booleanSet = new("Fact", Callback);
    booleanSet.AddItem("FakeSingleton");

    RealmEventArgs args = new(new EventTable { { "BooleanSet", booleanSet }, { "InitAtom", initAtom } });

    FakeSingleton singleton = new();
    singleton.Instance_OnGameInitialize(args);

    _callback.Should().BeTrue();
    //Assert.That(_callback, Is.True.After(250));
  }

  private static void Callback(RealmEventArgs args)
  {
    _callback = true;
  }
}