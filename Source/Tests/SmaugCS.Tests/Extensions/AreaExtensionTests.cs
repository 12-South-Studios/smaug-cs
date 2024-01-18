using FluentAssertions;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using Xunit;

namespace SmaugCS.Tests.Extensions
{

    public class AreaExtensionTests
    {
        private static AreaData _area;
        private static CharacterInstance _ch;

        public AreaExtensionTests()
        {
            LevelConstants.MaxLevel = 65;
            _area = new AreaData(1, "Test")
            {
                LowSoftRange = 5,
                HighSoftRange = 10,
                LowHardRange = 20,
                HighHardRange = 30
            };
            _ch = new CharacterInstance(1, "Tester");
        }

        [Theory]
        [InlineData(35, 35, ActFlags.IsNpc, true)]
        [InlineData(7, 7, ActFlags.NoWander, true)]
        [InlineData(1, 1, ActFlags.NoWander, false)]
        public void Area_InSoftRange_Test(int trust, int level, ActFlags flags, bool expectedValue)
        {
            _ch.Trust = trust;
            _ch.Level = level;
            _ch.Act.SetBit((int)flags);

            _area.IsInSoftRange(_ch).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(35, 35, ActFlags.IsNpc, true)]
        [InlineData(27, 27, ActFlags.NoWander, true)]
        [InlineData(1, 1, ActFlags.NoWander, false)]
        public void Area_InHardRange_Test(int trust, int level, ActFlags flag, bool expectedValue)
        {
            _ch.Trust = trust;
            _ch.Level = level;
            _ch.Act.SetBit((int)flag);

            _area.IsInHardRange(_ch).Should().Be(expectedValue);
        }
    }
}
