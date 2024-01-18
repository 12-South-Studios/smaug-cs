﻿using FluentAssertions;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using System.Collections.Generic;
using Xunit;

namespace SmaugCS.Tests
{

    public class CheckFunctionTests
    {
        [Fact]
        public void CheckIfNpc()
        {
            var actor = new CharacterInstance(1, "TestNpc");
            actor.Act.SetBit((int)ActFlags.IsNpc);

            CheckFunctions.CheckIfNpc(actor, actor).Should().BeTrue();
        }

        [Fact]
        public void CheckIfEmptyString()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            CheckFunctions.CheckIfEmptyString(actor, string.Empty, string.Empty).Should().BeTrue();
        }

        [Fact]
        public void CheckIfNullObject()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            CheckFunctions.CheckIfNullObject(actor, null, string.Empty).Should().BeTrue();
        }

        [Fact]
        public void CheckIfNotNullObject()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            CheckFunctions.CheckIfNotNullObject(actor, new object(), string.Empty).Should().BeTrue();
        }

        [Fact]
        public void CheckIfEquivalent()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            CheckFunctions.CheckIfEquivalent(actor, actor, actor, string.Empty).Should().BeTrue();
        }

        [Fact]
        public void CheckIf_NoArgs()
        {
            var actor = new CharacterInstance(1, "TestNpc") { PermanentStrength = 25 };

            CheckFunctions.CheckIf(actor, () => 5 * 10 == 50, string.Empty).Should().BeTrue();
        }

        [Fact]
        public void CheckIf_WithArgs()
        {
            var actor = new CharacterInstance(1, "TestNpc") { PermanentStrength = 25 };

            
                CheckFunctions.CheckIf(actor, args => ((CharacterInstance)args[0]).PermanentStrength == 25, string.Empty,
                    new List<object> { actor }).Should().BeTrue();
        }

        [Fact]
        public void CheckIfNotAuthorized_IsNpc()
        {
            var actor = new CharacterInstance(1, "TestNpc");
            actor.Act.SetBit((int)ActFlags.IsNpc);

            CheckFunctions.CheckIfNotAuthorized(actor, actor).Should().BeFalse();
        }

        [Fact]
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

            CheckFunctions.CheckIfNotAuthorized(actor, actor).Should().BeTrue();
        }

        [Fact]
        public void CheckIfSet()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            const int bitField = 2 | 4 | 8;

            CheckFunctions.CheckIfSet(actor, bitField, 4).Should().BeTrue();
        }

        [Fact]
        public void CheckIfNotSet()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            const int bitField = 2 | 4 | 8;

            CheckFunctions.CheckIfNotSet(actor, bitField, 16).Should().BeTrue();
        }

        [Fact]
        public void CheckIfTrue_ExpressionIsTrue()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            CheckFunctions.CheckIfTrue(actor, 5 > 1).Should().BeTrue();
        }

        [Fact]
        public void CheckIfTrue_ExpressionIsFalse()
        {
            var actor = new CharacterInstance(1, "TestNpc");

            CheckFunctions.CheckIfTrue(actor, 5 < 1).Should().BeFalse();
        }
    }
}
