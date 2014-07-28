using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class HelperFunctionTests
    {
        [TestCase(50, 5, true)]
        [TestCase(3, 5, false)]
        public void HasSufficientBloodPower(int currentBlood, int useBlood, bool expectedValue)
        {
            var actor = new CharacterInstance(1, "TestNpc")
            {
                CurrentRace = RaceTypes.Vampire,
                PlayerData = new PlayerData(1, 1)
                {
                    ConditionTable = new Dictionary<ConditionTypes, int>()
                    {
                        {ConditionTypes.Bloodthirsty, currentBlood}
                    }
                }
            };

            var list = new List<object> {actor, useBlood};

            Assert.That(Helpers.HelperFunctions.HasSufficientBloodPower(list), Is.EqualTo(expectedValue));
        }

        [TestCase(50, 5, true)]
        [TestCase(3, 5, false)]
        public void HasSufficientMana(int currentMana, int useMana, bool expectedValue)
        {
            var actor = new CharacterInstance(1, "TestNpc")
            {
                CurrentMana = currentMana
            };

            var list = new List<object> { actor, useMana };

            Assert.That(Helpers.HelperFunctions.HasSufficientMana(list), Is.EqualTo(expectedValue));
        }

        [TestCase(AffectedByTypes.Charm, true)]
        [TestCase(AffectedByTypes.Possess, true)]
        [TestCase(AffectedByTypes.Curse, false)]
        public void IsCharmedOrPossessed(AffectedByTypes affectedBy, bool expectedValue)
        {
            var actor = new CharacterInstance(1, "TestNpc");
            actor.Act = actor.Act.SetBit(ActFlags.IsNpc);
            actor.AffectedBy = actor.AffectedBy.SetBit(affectedBy);

            var list = new List<object> { actor };

            Assert.That(Helpers.HelperFunctions.IsCharmedOrPossessed(list), Is.EqualTo(expectedValue));
        }

        [Test]
        public void IsFighting()
        {
            var defender = new CharacterInstance(2, "TestNpc");

            var actor = new CharacterInstance(1, "TestNpc")
            {
                CurrentFighting = new FightingData
                {
                    Who = defender
                }
            };

            var list = new List<object> {actor};

            Assert.That(Helpers.HelperFunctions.IsFighting(list), Is.True);
        }

        [TestCase(PositionTypes.Aggressive, true)]
        [TestCase(PositionTypes.Berserk, true)]
        [TestCase(PositionTypes.Defensive, true)]
        [TestCase(PositionTypes.Evasive, true)]
        [TestCase(PositionTypes.Fighting, true)]
        [TestCase(PositionTypes.Dead, false)]
        public void IsInFightingPosition(PositionTypes position, bool expectedValue)
        {
            var actor = new CharacterInstance(1, "TestNpc")
            {
                CurrentPosition = position
            };

            var list = new List<object> {actor};

            Assert.That(Helpers.HelperFunctions.IsInFightingPosition(list), Is.EqualTo(expectedValue));
        }
    }
}
