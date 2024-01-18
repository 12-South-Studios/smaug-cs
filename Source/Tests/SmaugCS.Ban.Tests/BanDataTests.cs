
using FluentAssertions;
using SmaugCS.Common.Enumerations;
using System;
using Xunit;

namespace SmaugCS.Ban.Tests
{

    public class BanDataTests
    {
        [Theory]
        [InlineData(172800, false)]
        [InlineData(3600, true)]
        public void IsExpired_Test(int duration, bool expected)
        {
            var ban = new BanData
            {
                Id = 1,
                Type = BanTypes.Site,
                BannedOn = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)),
                Duration = duration
            };

            ban.IsExpired().Should().Be(expected);
        }

        [Fact]
        public void UnbanDate_Never_Test()
        {
            var ban = new BanData
            {
                Id = 1,
                Type = BanTypes.Site,
                BannedOn = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)),
                Duration = 0
            };

            ban.UnbanDate.Should().Be(DateTime.MaxValue);
            ban.IsExpired().Should().BeFalse();
        }
    }
}
