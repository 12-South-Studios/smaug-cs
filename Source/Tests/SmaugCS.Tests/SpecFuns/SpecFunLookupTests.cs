using FakeItEasy;
using FluentAssertions;
using SmaugCS.Data;
using SmaugCS.Repository;
using SmaugCS.SpecFuns;
using Xunit;

namespace SmaugCS.Tests.SpecFuns
{

    public class SpecFunLookupTests
    {
        private GenericRepository<SpecialFunction> SpecFunRepository;
        private IRepositoryManager MockDbManager;
        private SpecFunHandler Handler;

        public SpecFunLookupTests()
        {
            SpecFunRepository = new GenericRepository<SpecialFunction>();
            MockDbManager = A.Fake<IRepositoryManager>();
            Handler = new SpecFunHandler(MockDbManager);
        }

        private SpecialFunction GetSpecFun()
        {
            var specFun = new SpecialFunction(1, "spec_cast_adept");
            SpecFunRepository.Add(specFun.ID, specFun);
            return specFun;
        }

        [Fact]
        public void GetSpecFunReference_NoMatch_Test()
        {
            SpecFunHandler.GetSpecFunReference("invalid").Should().BeNull();
        }

        [Theory]
        [InlineData("spec_cast_adept", true)]
        [InlineData("invalid", false)]
        public void IsValidSpecFunTest(string specFun, bool expectedValue)
        {
            var expectedSpecFun = GetSpecFun();
            A.CallTo(() => MockDbManager.GetEntity<SpecialFunction>(A<string>.Ignored)).Returns(expectedSpecFun);

            Handler.IsValidSpecFun(specFun).Should().Be(expectedValue);
        }

        [Fact]
        public void GetSpecFun_Valid_Test()
        {
            var expectedSpecFun = GetSpecFun();
            A.CallTo(() => MockDbManager.GetEntity<SpecialFunction>(A<string>.Ignored)).Returns(expectedSpecFun);

            Handler.GetSpecFun("spec_cast_adept").Should().Be(expectedSpecFun);
        }

        [Fact]
        public void GetSpecFun_Invalid_Test()
        {
            GetSpecFun();
            A.CallTo(() => MockDbManager.GetEntity<SpecialFunction>(A<string>.Ignored)).Returns((SpecialFunction)null);

            Handler.GetSpecFun("spec_cast_cleric").Should().BeNull();
        }
    }
}
