using System.Collections.Generic;
using NUnit.Framework;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Helpers;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class CheckFunctionTests
    {
        [Test]
        public void CheckIfNpc()
        {
            var actor = new CharacterInstance(1, "TestNpc");
            actor.Act = actor.Act.SetBit(ActFlags.IsNpc);

            Assert.That(CheckFunctions.CheckIfNpc(actor, actor), Is.True);
        }

        [Test]
        public void CheckIfEmptyString()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            Assert.That(CheckFunctions.CheckIfEmptyString(actor, string.Empty, string.Empty), Is.True);
        }

        [Test]
        public void CheckIfNullObject()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            Assert.That(CheckFunctions.CheckIfNullObject(actor, null, string.Empty), Is.True);
        }

        [Test]
        public void CheckIfNotNullObject()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            Assert.That(CheckFunctions.CheckIfNotNullObject(actor, new object(), string.Empty), Is.True);
        }

        [Test]
        public void CheckIfEquivalent()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            Assert.That(CheckFunctions.CheckIfEquivalent(actor, actor, actor, string.Empty), Is.True);
        }

        [Test]
        public void CheckIf_NoArgs()
        {
            var actor = new CharacterInstance(1, "TestNpc") {PermanentStrength = 25};

            Assert.That(CheckFunctions.CheckIf(actor, () => 5 * 10 == 50, string.Empty), Is.True);
        }

        [Test]
        public void CheckIf_WithArgs()
        {
            var actor = new CharacterInstance(1, "TestNpc") {PermanentStrength = 25};

            Assert.That(
                CheckFunctions.CheckIf(actor, args => ((CharacterInstance)args[0]).PermanentStrength == 25, string.Empty,
                    new List<object> {actor}), Is.True);
        }

        [Test]
        public void CheckIfNotAuthorized_IsNpc()
        {
            var actor = new CharacterInstance(1, "TestNpc");
            actor.Act = actor.Act.SetBit(ActFlags.IsNpc);

            Assert.That(CheckFunctions.CheckIfNotAuthorized(actor, actor), Is.False);
        }

        [Test]
        public void CheckIfNotAuthorized_IsUnauthorized()
        {
            var actor = new PlayerInstance(1, "TestNpc")
            {
                PlayerData = new PlayerData(1, 1)
                {
                    AuthState = AuthorizationStates.Denied
                }
            };
            actor.PlayerData.Flags = actor.PlayerData.Flags.SetBit(PCFlags.Unauthorized);

            Assert.That(CheckFunctions.CheckIfNotAuthorized(actor, actor), Is.True);
        }

        [Test]
        public void CheckIfSet()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            const int bitField = 2 | 4 | 8;

            Assert.That(CheckFunctions.CheckIfSet(actor, bitField, 4), Is.True);
        }

        [Test]
        public void CheckIfNotSet()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            const int bitField = 2 | 4 | 8;

            Assert.That(CheckFunctions.CheckIfNotSet(actor, bitField, 16), Is.True);
        }

        [Test]
        public void CheckIfTrue_ExpressionIsTrue()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            Assert.That(CheckFunctions.CheckIfTrue(actor, 5 > 1), Is.True);
        }

        [Test]
        public void CheckIfTrue_ExpressionIsFalse()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            Assert.That(CheckFunctions.CheckIfTrue(actor, 5 < 1), Is.False);
        }
    }
}
