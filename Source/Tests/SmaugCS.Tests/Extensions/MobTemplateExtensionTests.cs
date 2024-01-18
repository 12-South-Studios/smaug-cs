using FluentAssertions;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Extensions;
using SmaugCS.Data.Templates;
using Xunit;

namespace SmaugCS.Tests.Extensions
{

    public class MobTemplateExtensionTests
    {
        [Fact]
        public void GetRaceTest()
        {
            var template = new MobileTemplate(1, "Test") { Race = "valatur" };

            template.GetRace().Should().Be((int)RaceTypes.Valatur);
        }

        [Fact]
        public void GetPositionTest()
        {
            var template = new MobileTemplate(1, "Test") { Position = "sitting" };

            template.GetPosition().Should().Be(PositionTypes.Sitting);
        }

        [Fact]
        public void GetDefensivePositionTest()
        {
            var template = new MobileTemplate(1, "Test") { DefensivePosition = "sitting" };

            template.GetDefensivePosition().Should().Be(PositionTypes.Sitting);
        }

        [Fact]
        public void GetGenderTest()
        {
            var template = new MobileTemplate(1, "Test") { Statistics = { [StatisticTypes.Gender] = "neuter" } };

            template.GetGender().Should().Be(GenderTypes.Neuter);
        }

        [Fact]
        public void GetResistanceTest()
        {
            var template = new MobileTemplate(1, "Test") { Resistance = "fire blunt" };

            var result = template.GetResistance();

            Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Fire).Should().BeTrue();
            Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Blunt).Should().BeTrue();
            Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Cold).Should().BeFalse();
        }

        [Fact]
        public void GetImmunityTest()
        {
            var template = new MobileTemplate(1, "Test") { Immunity = "fire blunt" };

            var result = template.GetImmunity();

            Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Fire).Should().BeTrue();
            Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Blunt).Should().BeTrue();
            Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Cold).Should().BeFalse();
        }

        [Fact]
        public void GetSusceptibilityTest()
        {
            var template = new MobileTemplate(1, "Test") { Susceptibility = "fire blunt" };

            var result = template.GetSusceptibility();

            Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Fire).Should().BeTrue();
            Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Blunt).Should().BeTrue();
            Common.NumberExtensions.IsSet(result, (int)ResistanceTypes.Cold).Should().BeFalse();
        }

    }
}
