using System.Collections.Generic;
using NUnit.Framework;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;

namespace SmaugCS.Tests
{
    [TestFixture]
    public class CheckFunctionTests
    {
        [Test]
        public void CheckIfNpc()
        {
            var actor = CharacterInstance.Create(1, "TestNpc");
            actor.Act = actor.Act.SetBit(ActFlags.IsNpc);

            Assert.That(CheckFunctions.CheckIfNpc(actor, actor), Is.True);
        }

        [Test]
        public void CheckIfEmptyString()
        {
            var actor = CharacterInstance.Create(1, "TestNpc");

            Assert.That(CheckFunctions.CheckIfEmptyString(actor, string.Empty, string.Empty), Is.True);
        }

        [Test]
        public void CheckIfNullObject()
        {
            var actor = CharacterInstance.Create(1, "TestNpc");

            Assert.That(CheckFunctions.CheckIfNullObject(actor, null, string.Empty), Is.True);
        }

        [Test]
        public void CheckIfNotNullObject()
        {
            var actor = CharacterInstance.Create(1, "TestNpc");

            Assert.That(CheckFunctions.CheckIfNotNullObject(actor, new object(), string.Empty), Is.True);
        }

        [Test]
        public void CheckIfEquivalent()
        {
            var actor = CharacterInstance.Create(1, "TestNpc");

            Assert.That(CheckFunctions.CheckIfEquivalent(actor, actor, actor, string.Empty), Is.True);
        }

        [Test]
        public void CheckIf_NoArgs()
        {
            var actor = CharacterInstance.Create(1, "TestNpc");
            actor.PermanentStrength = 25;

            Assert.That(CheckFunctions.CheckIf(actor, () => (5 * 10) == 50, string.Empty), Is.True);
        }

        [Test]
        public void CheckIf_WithArgs()
        {
            var actor = CharacterInstance.Create(1, "TestNpc");
            actor.PermanentStrength = 25;

            Assert.That(
                CheckFunctions.CheckIf(actor, args => ((CharacterInstance)args[0]).PermanentStrength == 25, string.Empty,
                    new List<object> {actor}), Is.True);
        }

        [Test]
        public void CheckIfNotAuthorized_IsNpc()
        {
            var actor = CharacterInstance.Create(1, "TestNpc");
            actor.Act = actor.Act.SetBit(ActFlags.IsNpc);

            Assert.That(CheckFunctions.CheckIfNotAuthorized(actor, actor), Is.False);
        }

        [Test]
        public void CheckIfNotAuthorized_HasInvalidAuthState()
        {
            var actor = CharacterInstance.Create(1, "TestNpc");
            actor.PlayerData = PlayerData.Create(1, 1);
            actor.PlayerData.AuthState = 5;

            Assert.That(CheckFunctions.CheckIfNotAuthorized(actor, actor), Is.False);
        }

        [Test]
        public void CheckIfNotAuthorized_IsUnauthorized()
        {
            var actor = CharacterInstance.Create(1, "TestNpc");
            actor.PlayerData = PlayerData.Create(1, 1);
            actor.PlayerData.AuthState = 2;
            actor.PlayerData.Flags = actor.PlayerData.Flags.SetBit((int) PCFlags.Unauthorized);

            Assert.That(CheckFunctions.CheckIfNotAuthorized(actor, actor), Is.True);
        }

        [Test]
        public void CheckIfSet()
        {
            var actor = CharacterInstance.Create(1, "TestNpc");

            const int bitField = 2 | 4 | 8;

            Assert.That(CheckFunctions.CheckIfSet(actor, bitField, 4), Is.True);
        }

        [Test]
        public void CheckIfNotSet()
        {
            var actor = CharacterInstance.Create(1, "TestNpc");

            const int bitField = 2 | 4 | 8;

            Assert.That(CheckFunctions.CheckIfNotSet(actor, bitField, 16), Is.True);
        }

        [Test]
        public void CheckIfTrue_ExpressionIsTrue()
        {
            var actor = CharacterInstance.Create(1, "TestNpc");

            Assert.That(CheckFunctions.CheckIfTrue(actor, 5 > 1), Is.True);
        }

        [Test]
        public void CheckIfTrue_ExpressionIsFalse()
        {
            var actor = CharacterInstance.Create(1, "TestNpc");

            Assert.That(CheckFunctions.CheckIfTrue(actor, 5 < 1), Is.False);
        }
    }
}
