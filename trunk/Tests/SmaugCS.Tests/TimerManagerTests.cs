using System.Timers;
using NUnit.Framework;
using SmaugCS.Managers;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class TimerManagerTests
    {
        private bool _callback;

        private void Callback(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _callback = true;
        }

        [Test]
        public void AddTimerTest()
        {
            var mgr = TimerManager.Instance;

            mgr.AddTimer(200, Callback);

            Assert.That(() => _callback, Is.True.After(250));
        }

        [Test]
        public void GetTimerTest()
        {
            var mgr = TimerManager.Instance;

            var id = mgr.AddTimer(200, Callback);

            var timer = mgr.GetTimer(id);

            Assert.That(timer.Id, Is.EqualTo(id));
        }

        [Test]
        public void DeleteTimerTest()
        {
            var mgr = TimerManager.Instance;

            var id = mgr.AddTimer(200, Callback);

            mgr.DeleteTimer(id);

            var timer = mgr.GetTimer(id);

            Assert.That(timer, Is.Null);
        }
    }
}
