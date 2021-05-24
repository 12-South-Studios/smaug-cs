using Moq;
using NUnit.Framework;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions.Character;
using SmaugCS.Repository;

namespace SmaugCS.Tests.Extensions
{
    [TestFixture]
    public class CharacterInstanceExtensionTests
    {
        private static CharacterInstance _ch;

        [SetUp]
        public void OnSetup()
        {
            LevelConstants.MaxLevel = 65;
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
            _ch.Act.SetBit(flags);
            Assert.That(_ch.IsNpc(), Is.EqualTo(expectedValue));
        }

        [Test]
        public void TimesKilled_NoMobPassed_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar");
            Assert.That(pch.TimesKilled(null), Is.EqualTo(0));
        }

        [Test]
        public void TimesKilled_MobIsPlayer_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar");
            MobileInstance mob = new MobileInstance(2, "TesterMob");
            Assert.That(pch.TimesKilled(mob), Is.EqualTo(0));
        }

        [Test]
        public void TimesKilled_NoMatch_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar");
            MobileInstance mob = new MobileInstance(2, "TesterMob");
            mob.Act.SetBit((int)ActFlags.IsNpc);
            mob.Parent = new MobileTemplate(2, "Template");

            pch.PlayerData = new PlayerData(1, 1);

            Assert.That(pch.TimesKilled(mob), Is.EqualTo(0));
        }

        [Test]
        public void TimesKilled_Match_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar");
            MobileInstance mob = new MobileInstance(2, "TesterMob");
            mob.Act.SetBit((int)ActFlags.IsNpc);
            mob.Parent = new MobileTemplate(2, "Template");

            pch.PlayerData = new PlayerData(1, 1);
            pch.PlayerData.Killed.Add(new KilledData(2));

            Assert.That(pch.TimesKilled(mob), Is.EqualTo(1));
        }

        [TestCase(ResistanceTypes.Unknown, false)]
        [TestCase(ResistanceTypes.Magic, true)]
        public void IsImmune_Test(ResistanceTypes type, bool expectedValue)
        {
            _ch.Immunity = _ch.Immunity.SetBit(type);

            Assert.That(_ch.IsImmune(type), Is.EqualTo(expectedValue));
        }

        [Test]
        public void IsImmune_LookupUnknown_Test()
        {
            var mockLookup = new Mock<ILookupManager>();
            mockLookup.Setup(x => x.GetResistanceType(It.IsAny<SpellDamageTypes>())).Returns(ResistanceTypes.Unknown);

            _ch.Immunity = _ch.Immunity.SetBit(ResistanceTypes.Magic);

            Assert.That(_ch.IsImmune(SpellDamageTypes.Poison, mockLookup.Object), Is.False);
        }

        [Test]
        public void IsImmune_Lookup_Test()
        {
            var mockLookup = new Mock<ILookupManager>();
            mockLookup.Setup(x => x.GetResistanceType(It.IsAny<SpellDamageTypes>())).Returns(ResistanceTypes.Magic);

            _ch.Immunity = _ch.Immunity.SetBit(ResistanceTypes.Magic);

            Assert.That(_ch.IsImmune(SpellDamageTypes.Poison, mockLookup.Object), Is.True);
        }

        [TestCase(AffectedByTypes.Flying, true)]
        [TestCase(AffectedByTypes.Floating, true)]
        [TestCase(AffectedByTypes.Blind, false)]
        public void IsFloating_Test(AffectedByTypes type, bool expectedValue)
        {
            _ch.AffectedBy.SetBit((int)type);

            Assert.That(_ch.IsFloating(), Is.EqualTo(expectedValue));
        }

        [TestCase(500, true)]
        [TestCase(-500, false)]
        public void IsGood_Test(int alignment, bool expectedValue)
        {
            _ch.CurrentAlignment = alignment;
            Assert.That(_ch.IsGood(), Is.EqualTo(expectedValue));
        }

        [TestCase(-500, true)]
        [TestCase(500, false)]
        public void IsEvil_Test(int alignment, bool expectedValue)
        {
            _ch.CurrentAlignment = alignment;
            Assert.That(_ch.IsEvil(), Is.EqualTo(expectedValue));
        }

        [TestCase(0, true)]
        [TestCase(-500, false)]
        [TestCase(500, false)]
        public void IsNeutral_Test(int alignment, bool expectedValue)
        {
            _ch.CurrentAlignment = alignment;
            Assert.That(_ch.IsNeutral(), Is.EqualTo(expectedValue));
        }

        [TestCase(PositionTypes.Incapacitated, false)]
        [TestCase(PositionTypes.Sleeping, false)]
        [TestCase(PositionTypes.Fighting, true)]
        [TestCase(PositionTypes.Mounted, true)]
        public void IsAwake_Test(PositionTypes position, bool expectedValue)
        {
            _ch.CurrentPosition = position;
            Assert.That(_ch.IsAwake(), Is.EqualTo(expectedValue));
        }

        [TestCase(ActFlags.IsNpc, RaceTypes.Caorlei, ClassTypes.Cleric, false)]
        [TestCase(ActFlags.Immortal, RaceTypes.Vampire, ClassTypes.Cleric, true)]
        [TestCase(ActFlags.Immortal, RaceTypes.Caorlei, ClassTypes.Vampire, true)]
        public void IsVampire_Test(ActFlags actFlag, RaceTypes race, ClassTypes cls, bool expectedValue)
        {
            _ch.Act.SetBit((int)actFlag);
            _ch.CurrentRace = race;
            _ch.CurrentClass = cls;
            Assert.That(_ch.IsVampire(), Is.EqualTo(expectedValue));
        }

        [Test]
        public void IsDevoted_IsNpc_Test()
        {
            MobileInstance mob = new MobileInstance(1, "TestChar");
            mob.Act.SetBit((int)ActFlags.IsNpc);
            Assert.That(mob.IsDevoted(), Is.False);
        }

        [Test]
        public void IsDevoted_NoDeity_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar")
            {
                PlayerData = new PlayerData(1, 1)
            };
            Assert.That(pch.IsDevoted(), Is.False);
        }

        [Test]
        public void IsDevoted_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar")
            {
                PlayerData = new PlayerData(1, 1)
                {
                    CurrentDeity = new DeityData(1, "Test")
                }
            };
            Assert.That(pch.IsDevoted(), Is.True);
        }

        [TestCase(RoomFlags.Dark, true)]
        [TestCase(RoomFlags.Indoors, false)]
        [TestCase(RoomFlags.Tunnel, false)]
        public void IsOutside_Test(RoomFlags flag, bool expectedValue)
        {
            _ch.CurrentRoom = new RoomTemplate(1, "Test");
            _ch.CurrentRoom.Flags = _ch.CurrentRoom.Flags.SetBit(flag);
            Assert.That(_ch.IsOutside(), Is.EqualTo(expectedValue));
        }

        [TestCase(false, ClassTypes.Warrior, false)]
        [TestCase(true, ClassTypes.Mage, true)]
        public void CanCast_Test(bool isSpellcaster, ClassTypes clsType, bool expectedValue)
        {
            var classRepo = new GenericRepository<ClassData>();
            classRepo.Add(1, new ClassData(1, "Test") { IsSpellcaster = isSpellcaster, Type = clsType });

            var mockDb = new Mock<IRepositoryManager>();
            mockDb.SetupGet(x => x.CLASSES).Returns(classRepo);

            _ch.CurrentClass = clsType;

            Assert.That(_ch.CanCast(mockDb.Object), Is.EqualTo(expectedValue));
        }

        [Test]
        public void IsRetired_NoData_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar");
            Assert.That(pch.IsRetired(), Is.False);
        }

        [Test]
        public void IsRetired_NoFlag_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar")
            {
                PlayerData = new PlayerData(1, 1)
            };
            Assert.That(pch.IsRetired(), Is.False);
        }

        [Test]
        public void IsRetired_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar")
            {
                PlayerData = new PlayerData(1, 1)
            };
            pch.PlayerData.Flags = pch.PlayerData.Flags.SetBit(PCFlags.Retired);
            Assert.That(pch.IsRetired(), Is.True);
        }

        [Test]
        public void IsGuest_NoData_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar");
            Assert.That(pch.IsGuest(), Is.False);
        }

        [Test]
        public void IsGuest_NoFlag_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar")
            {
                PlayerData = new PlayerData(1, 1)
            };
            Assert.That(pch.IsGuest(), Is.False);
        }

        [Test]
        public void IsGuest_Test()
        {
            PlayerInstance pch = new PlayerInstance(1, "TestChar")
            {
                PlayerData = new PlayerData(1, 1)
            };
            pch.PlayerData.Flags = pch.PlayerData.Flags.SetBit(PCFlags.Guest);
            Assert.That(pch.IsGuest(), Is.True);
        }
    }
}
