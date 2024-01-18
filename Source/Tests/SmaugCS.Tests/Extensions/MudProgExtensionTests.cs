using FakeItEasy;
using FluentAssertions;
using Realm.Library.Lua;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;
using SmaugCS.Logging;
using System;
using Xunit;

namespace SmaugCS.Tests.Extensions
{

    public class MudProgExtensionTests
    {
        private static ILogManager _mockedLogManager;
        private static ILuaManager _mockedLuaManager;

        public MudProgExtensionTests()
        {
            _mockedLogManager = A.Fake<ILogManager>();
            _mockedLuaManager = A.Fake<ILuaManager>();
        }

        [Fact]
        public void Execute_IsCharmed_Test()
        {
            var actor = new CharacterInstance(1, "TestChar");
            actor.AffectedBy.SetBit((int)AffectedByTypes.Charm);

            var mprog = new MudProgData { Type = MudProgTypes.Act };

            mprog.Execute(actor, _mockedLuaManager, _mockedLogManager).Should().BeFalse();
        }

        [Fact]
        public void Execute_IsFileProg_Test()
        {
            var actor = new CharacterInstance(1, "TestChar");

            var mprog = new MudProgData { Type = MudProgTypes.Act, IsFileProg = true };

            Action act = () => mprog.Execute(actor, _mockedLuaManager, _mockedLogManager);
            act.Should().Throw<NotImplementedException>();
        }

        [Fact]
        public void Execute_CatchesAndLogsException_Test()
        {
            bool callback = false;

            var actor = new CharacterInstance(1, "TestChar");

            var mprog = new MudProgData { Type = MudProgTypes.Act };

            A.CallTo(() => _mockedLuaManager.DoLuaScript(A<string>.Ignored)).Throws(new LuaException("Test Exception"));
            A.CallTo(() => _mockedLogManager.Error(A<LuaException>.Ignored)).Invokes(() => callback = true);

            mprog.Execute(actor, _mockedLuaManager, _mockedLogManager).Should().BeFalse();
            callback.Should().BeTrue();
        }

        [Fact]
        public void Execute_Successful_Test()
        {
            var actor = new CharacterInstance(1, "TestChar");

            var mprog = new MudProgData { Type = MudProgTypes.Act };

            mprog.Execute(actor, _mockedLuaManager, _mockedLogManager).Should().BeTrue();
        }
    }
}
