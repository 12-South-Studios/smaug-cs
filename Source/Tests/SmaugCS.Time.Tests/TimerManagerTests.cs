using Ninject;
using System.Timers;
using FakeItEasy;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;

namespace SmaugCS.Time.Tests
{

    public class TimerManagerTests
    {
        private bool _callback;
        private static ITimerManager _mgr;

        public TimerManagerTests()
        {
            _mgr = new TimerManager(A.Fake<IKernel>());
        }

        private void Callback(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _callback = true;
        }

        [Fact]
        public void GetTimerTest()
        {
            var id = _mgr.AddTimer(200, Callback);

            var timer = _mgr.GetTimer(id);

            timer.Id.Should().Be(id);
        }

        [Fact]
        public void DeleteTimerTest()
        {
            var id = _mgr.AddTimer(200, Callback);

            _mgr.DeleteTimer(id);

            var timer = _mgr.GetTimer(id);

            timer.Should().BeNull();
        }
    }
}
