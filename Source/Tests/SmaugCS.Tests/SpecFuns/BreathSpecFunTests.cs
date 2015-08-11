using Moq;
using NUnit.Framework;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.SpecFuns;
using SmaugCS.SpecFuns.Breaths;

namespace SmaugCS.Tests.SpecFuns
{
    [TestFixture]
    public class BreathSpecFunTests
    {
        private static MobileInstance _character;

        [SetUp]
        public void OnSetup()
        {
            _character = new MobileInstance(1, "Tester");
        }

        [Test]
        public void DoBreathAny_NotFighting_Test()
        {
            _character.CurrentPosition = PositionTypes.Incapacitated;

            var mockDbManager = new Mock<IManager>();

            Assert.That(BreathAny.Execute(_character, mockDbManager.Object), Is.EqualTo(false));
        }
    }
}
