using System;
using NUnit.Framework;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.SpecFuns;

namespace SmaugCS.Tests.SpecFuns
{
    [TestFixture]
    public class BreathSpecFunTests
    {
        private static CharacterInstance _character;

        [SetUp]
        public void OnSetup()
        {
            _character = new CharacterInstance(1, "Tester");
        }

        [Test]
        public void DoBreathAny_NotFighting_Test()
        {
            _character.CurrentPosition = PositionTypes.Incapacitated;

            Assert.That(BreathAny.DoSpecBreathAny(_character), Is.EqualTo(false));
        }


    }
}
