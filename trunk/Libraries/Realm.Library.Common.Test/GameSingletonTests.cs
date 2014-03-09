using Moq;
using NUnit.Framework;
using Realm.Library.Common.Data;
using Realm.Library.Common.Logging;
using log4net;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class GameSingletonTests
    {
        private static bool _callback;

        private class FakeSingleton : GameSingleton
        {
        }

        [SetUp]
        public void OnSetup()
        {
            _callback = false;
        }

        [Test]
        public void OnGameInitialize_Test()
        {
            var mockLogger = new Mock<LogWrapper>(new object[] {new Mock<ILog>().Object, LogLevel.Info});
            mockLogger.Setup(x => x.InfoFormat(It.IsAny<string>(), It.IsAny<object>()));

            var initAtom = new DictionaryAtom();
            initAtom.Add(new StringAtom("Logger"), new ObjectAtom(mockLogger.Object));

            var booleanSet = new BooleanSet("Test", Callback);
            booleanSet.AddItem("FakeSingleton");

            var args = new RealmEventArgs(new EventTable {{"BooleanSet", booleanSet}, {"InitAtom", initAtom}});

            var singleton = new FakeSingleton();
            singleton.Instance_OnGameInitialize(args);

            Assert.That(_callback, Is.True.After(250));
        }

        private static void Callback(RealmEventArgs args)
        {
            _callback = true;
        }
    }
}
