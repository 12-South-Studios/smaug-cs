using System.Timers;
using NUnit.Framework;
using SmaugCS.Managers;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class TimerManagerTests
    {
        private bool _callback;
        private static TimerManager _mgr;

        [SetUp]
        public void OnSetup()
        {
            _mgr = TimerManager.Instance;
        }

        private void Callback(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _callback = true;
        }

        [Test]
        public void AddTimerTest()
        {
            _mgr.AddTimer(200, Callback);

            Assert.That(() => _callback, Is.True.After(250));
        }

        [Test]
        public void GetTimerTest()
        {
            var id = _mgr.AddTimer(200, Callback);

            var timer = _mgr.GetTimer(id);

            Assert.That(timer.Id, Is.EqualTo(id));
        }

        [Test]
        public void DeleteTimerTest()
        {
            var id = _mgr.AddTimer(200, Callback);

            _mgr.DeleteTimer(id);

            var timer = _mgr.GetTimer(id);

            Assert.That(timer, Is.Null);
        }
    }
}
