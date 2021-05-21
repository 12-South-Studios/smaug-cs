using NUnit.Framework;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Extensions;
using SmaugCS.Data.Templates;

namespace SmaugCS.Tests.Extensions
{
    [TestFixture]
    public class MobTemplateExtensionTests
    {
        [Test]
        public void GetRaceTest()
        {
            var template = new MobileTemplate(1, "Test") { Race = "valatur" };

            Assert.That(template.GetRace(), Is.EqualTo((int)RaceTypes.Valatur));
        }

        [Test]
        public void GetPositionTest()
        {
            var template = new MobileTemplate(1, "Test") { Position = "sitting" };

            Assert.That(template.GetPosition(), Is.EqualTo(PositionTypes.Sitting));
        }

        [Test]
        public void GetDefensivePositionTest()
        {
            var template = new MobileTemplate(1, "Test") { DefensivePosition = "sitting" };

            Assert.That(template.GetDefensivePosition(), Is.EqualTo(PositionTypes.Sitting));
        }

        [Test]
        public void GetGenderTest()
        {
            var template = new MobileTemplate(1, "Test") { Statistics = { [StatisticTypes.Gender] = "neuter" } };

            Assert.That(template.GetGender(), Is.EqualTo(GenderTypes.Neuter));
        }

        [Test]
        public void GetResistanceTest()
        {
            var template = new MobileTemplate(1, "Test") { Resistance = "fire blunt" };

            var result = template.GetResistance();

            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Fire), Is.True);
            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Blunt), Is.True);
            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Cold), Is.False);
        }

        [Test]
        public void GetImmunityTest()
        {
            var template = new MobileTemplate(1, "Test") { Immunity = "fire blunt" };

            var result = template.GetImmunity();

            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Fire), Is.True);
            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Blunt), Is.True);
            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Cold), Is.False);
        }

        [Test]
        public void GetSusceptibilityTest()
        {
            var template = new MobileTemplate(1, "Test") { Susceptibility = "fire blunt" };

            var result = template.GetSusceptibility();

            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Fire), Is.True);
            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Blunt), Is.True);
            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Cold), Is.False);
        }

    }
}
