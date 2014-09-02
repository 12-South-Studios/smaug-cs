using NUnit.Framework;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Tests.Extensions
{
    [TestFixture]
    public class MobTemplateExtensionTests
    {
        [Test]
        public void GetRaceTest()
        {
            var template = new MobTemplate(1, "Test");
            template.Race = "valatur";

            Assert.That(template.GetRace(), Is.EqualTo((int)RaceTypes.Valatur));
        }

        [Test]
        public void GetPositionTest()
        {
            var template = new MobTemplate(1, "Test");
            template.Position = "sitting";

            Assert.That(template.GetPosition(), Is.EqualTo(PositionTypes.Sitting));
        }

        [Test]
        public void GetDefensivePositionTest()
        {
            var template = new MobTemplate(1, "Test");
            template.DefensivePosition = "sitting";

            Assert.That(template.GetDefensivePosition(), Is.EqualTo(PositionTypes.Sitting));
        }

        [Test]
        public void GetGenderTest()
        {
            var template = new MobTemplate(1, "Test");
            template.Gender = "neuter";

            Assert.That(template.GetGender(), Is.EqualTo(GenderTypes.Neuter));
        }

        [Test]
        public void GetResistanceTest()
        {
            var template = new MobTemplate(1, "Test");
            template.Resistance = "fire blunt";

            var result = template.GetResistance();

            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Fire), Is.True);
            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Blunt), Is.True);
            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Cold), Is.False);
        }

        [Test]
        public void GetImmunityTest()
        {
            var template = new MobTemplate(1, "Test");
            template.Immunity = "fire blunt";

            var result = template.GetImmunity();

            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Fire), Is.True);
            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Blunt), Is.True);
            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Cold), Is.False);
        }

        [Test]
        public void GetSusceptibilityTest()
        {
            var template = new MobTemplate(1, "Test");
            template.Susceptibility = "fire blunt";

            var result = template.GetSusceptibility();

            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Fire), Is.True);
            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Blunt), Is.True);
            Assert.That(Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Cold), Is.False);
        }

    }
}
