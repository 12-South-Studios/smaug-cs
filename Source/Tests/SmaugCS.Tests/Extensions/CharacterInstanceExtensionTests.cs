using FakeItEasy;
using FluentAssertions;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;
using SmaugCS.Data.Templates;
using SmaugCS.Repository;
using Xunit;

namespace SmaugCS.Tests.Extensions
{

    public class CharacterInstanceExtensionTests
    {
        private static CharacterInstance _ch;

        public CharacterInstanceExtensionTests()
        {
            LevelConstants.MaxLevel = 65;
            _ch = new CharacterInstance(1, "Tester");
        }

        [Theory]
        [InlineData(PositionTypes.Aggressive, true)]
        [InlineData(PositionTypes.Berserk, true)]
        [InlineData(PositionTypes.Defensive, true)]
        [InlineData(PositionTypes.Fighting, true)]
        [InlineData(PositionTypes.Evasive, true)]
        [InlineData(PositionTypes.Dead, false)]
        public void IsInCombatPosition_Test(PositionTypes currentPos, bool expectedValue)
        {
            _ch.CurrentPosition = currentPos;
            _ch.IsInCombatPosition().Should().Be(expectedValue);
        }

        [Theory]
        [InlineData((int)(ActFlags.Pet | ActFlags.Immortal), false)]
        [InlineData((int)ActFlags.IsNpc, true)]
        public void IsNpc_Test(int flags, bool expectedValue)
        {
            _ch.Act.SetBit(flags);
            _ch.IsNpc().Should().Be(expectedValue);
        }

        [Fact]
        public void TimesKilled_NoMobPassed_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar");
            pch.TimesKilled(null).Should().Be(0);
        }

        [Fact]
        public void TimesKilled_MobIsPlayer_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar");
            MobileInstance mob = new MobileInstance(2, "TesterMob");
            pch.TimesKilled(mob).Should().Be(0);
        }

        [Fact]
        public void TimesKilled_NoMatch_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar");
            MobileInstance mob = new MobileInstance(2, "TesterMob");
            mob.Act.SetBit((int)ActFlags.IsNpc);
            mob.Parent = new MobileTemplate(2, "Template");

            pch.PlayerData = new PlayerData(1, 1);

            pch.TimesKilled(mob).Should().Be(0);
        }

        [Fact]
        public void TimesKilled_Match_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar");
            MobileInstance mob = new MobileInstance(2, "TesterMob");
            mob.Act.SetBit((int)ActFlags.IsNpc);
            mob.Parent = new MobileTemplate(2, "Template");

            pch.PlayerData = new PlayerData(1, 1);
            pch.PlayerData.Killed.Add(new KilledData(2));

            pch.TimesKilled(mob).Should().Be(1);
        }

        [Theory]
        [InlineData(ResistanceTypes.Unknown, false)]
        [InlineData(ResistanceTypes.Magic, true)]
        public void IsImmune_Test(ResistanceTypes type, bool expectedValue)
        {
            _ch.Immunity = _ch.Immunity.SetBit(type);

            _ch.IsImmune(type).Should().Be(expectedValue);
        }

        [Fact]
        public void IsImmune_LookupUnknown_Test()
        {
            var mockLookup = A.Fake<ILookupManager>();
            A.CallTo(() => mockLookup.GetResistanceType(A<SpellDamageTypes>.Ignored)).Returns(ResistanceTypes.Unknown);

            _ch.Immunity = _ch.Immunity.SetBit(ResistanceTypes.Magic);

            _ch.IsImmune(SpellDamageTypes.Poison, mockLookup).Should().BeFalse();
        }

        [Fact]
        public void IsImmune_Lookup_Test()
        {
            var mockLookup = A.Fake<ILookupManager>();
            A.CallTo(() => mockLookup.GetResistanceType(A<SpellDamageTypes>.Ignored)).Returns(ResistanceTypes.Magic);

            _ch.Immunity = _ch.Immunity.SetBit(ResistanceTypes.Magic);

            _ch.IsImmune(SpellDamageTypes.Poison, mockLookup).Should().BeTrue();
        }

        [Theory]
        [InlineData(AffectedByTypes.Flying, true)]
        [InlineData(AffectedByTypes.Floating, true)]
        [InlineData(AffectedByTypes.Blind, false)]
        public void IsFloating_Test(AffectedByTypes type, bool expectedValue)
        {
            _ch.AffectedBy.SetBit((int)type);

            _ch.IsFloating().Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(500, true)]
        [InlineData(-500, false)]
        public void IsGood_Test(int alignment, bool expectedValue)
        {
            _ch.CurrentAlignment = alignment;
            _ch.IsGood().Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(-500, true)]
        [InlineData(500, false)]
        public void IsEvil_Test(int alignment, bool expectedValue)
        {
            _ch.CurrentAlignment = alignment;
            _ch.IsEvil().Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(-500, false)]
        [InlineData(500, false)]
        public void IsNeutral_Test(int alignment, bool expectedValue)
        {
            _ch.CurrentAlignment = alignment;
            _ch.IsNeutral().Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(PositionTypes.Incapacitated, false)]
        [InlineData(PositionTypes.Sleeping, false)]
        [InlineData(PositionTypes.Fighting, true)]
        [InlineData(PositionTypes.Mounted, true)]
        public void IsAwake_Test(PositionTypes position, bool expectedValue)
        {
            _ch.CurrentPosition = position;
            _ch.IsAwake().Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(ActFlags.IsNpc, RaceTypes.Caorlei, ClassTypes.Cleric, false)]
        [InlineData(ActFlags.Immortal, RaceTypes.Vampire, ClassTypes.Cleric, true)]
        [InlineData(ActFlags.Immortal, RaceTypes.Caorlei, ClassTypes.Vampire, true)]
        public void IsVampire_Test(ActFlags actFlag, RaceTypes race, ClassTypes cls, bool expectedValue)
        {
            _ch.Act.SetBit((int)actFlag);
            _ch.CurrentRace = race;
            _ch.CurrentClass = cls;
            _ch.IsVampire().Should().Be(expectedValue);
        }

        [Fact]
        public void IsDevoted_IsNpc_Test()
        {
            MobileInstance mob = new MobileInstance(1, "TestChar");
            mob.Act.SetBit((int)ActFlags.IsNpc);
            mob.IsDevoted().Should().BeFalse();
        }

        [Fact]
        public void IsDevoted_NoDeity_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar")
            {
                PlayerData = new PlayerData(1, 1)
            };
            pch.IsDevoted().Should().BeFalse();
        }

        [Fact]
        public void IsDevoted_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar")
            {
                PlayerData = new PlayerData(1, 1)
                {
                    CurrentDeity = new DeityData(1, "Test")
                }
            };
            pch.IsDevoted().Should().BeTrue();
        }

        [Theory]
        [InlineData(RoomFlags.Dark, true)]
        [InlineData(RoomFlags.Indoors, false)]
        [InlineData(RoomFlags.Tunnel, false)]
        public void IsOutside_Test(RoomFlags flag, bool expectedValue)
        {
            _ch.CurrentRoom = new RoomTemplate(1, "Test");
            _ch.CurrentRoom.Flags = _ch.CurrentRoom.Flags.SetBit(flag);
            _ch.IsOutside().Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(false, ClassTypes.Warrior, false)]
        [InlineData(true, ClassTypes.Mage, true)]
        public void CanCast_Test(bool isSpellcaster, ClassTypes clsType, bool expectedValue)
        {
            var classRepo = new GenericRepository<ClassData>();
            classRepo.Add(1, new ClassData(1, "Test") { IsSpellcaster = isSpellcaster, Type = clsType });

            var mockDb = A.Fake<IRepositoryManager>();
            A.CallTo(() => mockDb.CLASSES).Returns(classRepo);

            _ch.CurrentClass = clsType;

            _ch.CanCast(mockDb).Should().Be(expectedValue);
        }

        [Fact]
        public void IsRetired_NoData_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar");
            pch.IsRetired().Should().BeFalse();
        }

        [Fact]
        public void IsRetired_NoFlag_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar")
            {
                PlayerData = new PlayerData(1, 1)
            };
            pch.IsRetired().Should().BeFalse();
        }

        [Fact]
        public void IsRetired_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar")
            {
                PlayerData = new PlayerData(1, 1)
            };
            pch.PlayerData.Flags = pch.PlayerData.Flags.SetBit(PCFlags.Retired);
            pch.IsRetired().Should().BeTrue();
        }

        [Fact]
        public void IsGuest_NoData_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar");
            pch.IsGuest().Should().BeFalse();
        }

        [Fact]
        public void IsGuest_NoFlag_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar")
            {
                PlayerData = new PlayerData(1, 1)
            };
            pch.IsGuest().Should().BeFalse();
        }

        [Fact]
        public void IsGuest_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar")
            {
                PlayerData = new PlayerData(1, 1)
            };
            pch.PlayerData.Flags = pch.PlayerData.Flags.SetBit(PCFlags.Guest);
            pch.IsGuest().Should().BeTrue();
        }
    }
}
