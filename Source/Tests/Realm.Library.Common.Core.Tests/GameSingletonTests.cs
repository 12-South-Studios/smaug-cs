using Realm.Library.Common.Data;
using Realm.Library.Common.Logging;
using log4net;
using Realm.Library.Common.Events;
using Realm.Library.Common.Objects;
using Xunit;
using FakeItEasy;
using FluentAssertions;

namespace Realm.Library.Common.Fact
{
    public class GameSingletonFacts
    {
        public static bool _callback;

        private class FakeSingleton : GameSingleton
        {
        }

        [Fact]
        public void OnGameInitialize_Fact()
        {
            _callback = false;

            var logger = A.Fake<LogWrapper>();

            var initAtom = new DictionaryAtom();
            initAtom.Add(new StringAtom("Logger"), new ObjectAtom(logger));

            var booleanSet = new BooleanSet("Fact", Callback);
            booleanSet.AddItem("FakeSingleton");

            var args = new RealmEventArgs(new EventTable {{"BooleanSet", booleanSet}, {"InitAtom", initAtom}});

            var singleton = new FakeSingleton();
            singleton.Instance_OnGameInitialize(args);

            _callback.Should().BeTrue();
            //Assert.That(_callback, Is.True.After(250));
        }

        private static void Callback(RealmEventArgs args)
        {
            _callback = true;
        }
    }
}
