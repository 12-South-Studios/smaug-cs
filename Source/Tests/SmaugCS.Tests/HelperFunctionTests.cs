using FluentAssertions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using System.Collections.Generic;
using Xunit;

namespace SmaugCS.Tests
{

    public class HelperFunctionTests
    {
        [Theory]
        [InlineData(50, 5, true)]
        [InlineData(3, 5, false)]
        public void HasSufficientBloodPower(int currentBlood, int useBlood, bool expectedValue)
        {
            var actor = new PlayerInstance(1, "TestNpc")
            {
                CurrentRace = RaceTypes.Vampire,
                PlayerData = new PlayerData(1, 1)
            };
            actor.PlayerData.ConditionTable.Clear();
            actor.PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] = currentBlood;

            var list = new List<object> { actor, useBlood };

            HelperFunctions.HasSufficientBloodPower(list).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(50, 5, true)]
        [InlineData(3, 5, false)]
        public void HasSufficientMana(int currentMana, int useMana, bool expectedValue)
        {
            var actor = new CharacterInstance(1, "TestNpc")
            {
                CurrentMana = currentMana
            };

            var list = new List<object> { actor, useMana };

            HelperFunctions.HasSufficientMana(list).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(AffectedByTypes.Charm, true)]
        [InlineData(AffectedByTypes.Possess, true)]
        [InlineData(AffectedByTypes.Curse, false)]
        public void IsCharmedOrPossessed(AffectedByTypes affectedBy, bool expectedValue)
        {
            var actor = new CharacterInstance(1, "TestNpc");
            actor.Act.SetBit((int)ActFlags.IsNpc);
            actor.AffectedBy.SetBit((int)affectedBy);

            var list = new List<object> { actor };

            HelperFunctions.IsCharmedOrPossessed(list).Should().Be(expectedValue);
        }

        [Fact]
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

            var list = new List<object> { actor };

            HelperFunctions.IsFighting(list).Should().BeTrue();
        }

        [Theory]
        [InlineData(PositionTypes.Aggressive, true)]
        [InlineData(PositionTypes.Berserk, true)]
        [InlineData(PositionTypes.Defensive, true)]
        [InlineData(PositionTypes.Evasive, true)]
        [InlineData(PositionTypes.Fighting, true)]
        [InlineData(PositionTypes.Dead, false)]
        public void IsInFightingPosition(PositionTypes position, bool expectedValue)
        {
            var actor = new CharacterInstance(1, "TestNpc") { CurrentPosition = position };

            var list = new List<object> { actor };

            HelperFunctions.IsInFightingPosition(list).Should().Be(expectedValue);
        }
    }
}
