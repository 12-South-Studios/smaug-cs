using NUnit.Framework;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Tests.Extensions
{
    [TestFixture]
    public class AreaExtensionTests
    {
        private static AreaData _area;
        private static CharacterInstance _ch;

        [SetUp]
        public void OnSetup()
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

        [TestCase(55, 55, ActFlags.NoWander, true)]
        [TestCase(35, 35, ActFlags.IsNpc, true)]
        [TestCase(7, 7, ActFlags.NoWander, true)]
        [TestCase(1, 1, ActFlags.NoWander, false)]
        public void Area_InSoftRange_Test(int trust, int level, ActFlags flags, bool expectedValue)
        {
            _ch.Trust = trust;
            _ch.Level = level;
            _ch.Act = _ch.Act.SetBit(flags);

            Assert.That(_area.InSoftRange(_ch), Is.EqualTo(expectedValue));
        }

        [TestCase(55, 55, ActFlags.NoWander, true)]
        [TestCase(35, 35, ActFlags.IsNpc, true)]
        [TestCase(27, 27, ActFlags.NoWander, true)]
        [TestCase(1, 1, ActFlags.NoWander, false)]
        public void Area_InHardRange_Test(int trust, int level, ActFlags flags, bool expectedValue)
        {
            _ch.Trust = trust;
            _ch.Level = level;
            _ch.Act = _ch.Act.SetBit(flags);

            Assert.That(_area.InHardRange(_ch), Is.EqualTo(expectedValue));
        }
    }
}
