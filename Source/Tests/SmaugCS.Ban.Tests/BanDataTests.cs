using System;
using NUnit.Framework;
using SmaugCS.Common.Enumerations;

namespace SmaugCS.Ban.Tests
{
    [TestFixture]
    public class BanDataTests
    {
        [TestCase(172800, false)]
        [TestCase(3600, true)]
        public void IsExpired_Test(int duration, bool expected)
        {
            var ban = new BanData(1, BanTypes.Site)
            {
                BannedOn = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)),
                Duration = duration
            };

            Assert.That(ban.IsExpired(), Is.EqualTo(expected));
        }

        [Test]
        public void UnbanDate_Never_Test()
        {
            var ban = new BanData(1, BanTypes.Site)
            {
                BannedOn = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)),
                Duration = 0
            };

            Assert.That(ban.UnbanDate, Is.EqualTo(DateTime.MaxValue));
            Assert.That(ban.IsExpired(), Is.False);
        }
    }
}
