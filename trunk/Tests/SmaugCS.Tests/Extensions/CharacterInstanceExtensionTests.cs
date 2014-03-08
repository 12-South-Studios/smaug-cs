using System;
using NUnit.Framework;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.Tests.Extensions
{
    [TestFixture]
    public class CharacterInstanceExtensionTests
    {
        private static CharacterInstance _ch;

        [SetUp]
        public void OnSetup()
        {
            LevelConstants.MAX_LEVEL = 65;
            _ch = new CharacterInstance(1, "Tester");
        }

        [TestCase(PositionTypes.Aggressive, true)]
        [TestCase(PositionTypes.Berserk, true)]
        [TestCase(PositionTypes.Defensive, true)]
        [TestCase(PositionTypes.Fighting, true)]
        [TestCase(PositionTypes.Evasive, true)]
        [TestCase(PositionTypes.Dead, false)]
        public void IsInCombatPosition_Test(PositionTypes currentPos, bool expectedValue)
        {
            _ch.CurrentPosition = currentPos;
            Assert.That(_ch.IsInCombatPosition(), Is.EqualTo(expectedValue));
        }

        [TestCase(ActFlags.Pet | ActFlags.Immortal, false)]
        [TestCase(ActFlags.IsNpc, true)]
        public void IsNpc_Test(int flags, bool expectedValue)
        {
            _ch.Act.SetBit((ulong)flags);
            Assert.That(_ch.IsNpc(), Is.EqualTo(expectedValue));
        }

        [TestCase(AffectedByTypes.Blind | AffectedByTypes.Invisible, AffectedByTypes.DetectEvil, false)]
        [TestCase(AffectedByTypes.Blind | AffectedByTypes.Invisible, AffectedByTypes.Blind, true)]
        public void IsAffected_Test(int flags, AffectedByTypes affectedBy, bool expectedValue)
        {
            _ch.AffectedBy.SetBit(flags);
            Assert.That(_ch.IsAffected(affectedBy), Is.EqualTo(expectedValue));
        }
    }
}
