using Moq;
using NUnit.Framework;
using SmaugCS.Data;
using SmaugCS.Repository;
using SmaugCS.SpecFuns;

namespace SmaugCS.Tests.SpecFuns
{
    [TestFixture]
    public class SpecFunLookupTests
    {
        private GenericRepository<SpecialFunction> SpecFunRepository;
        private Mock<IRepositoryManager> MockDbManager;
        private SpecFunHandler Handler;

        [SetUp]
        public void OnSetup()
        {
            SpecFunRepository = new GenericRepository<SpecialFunction>();
            MockDbManager = new Mock<IRepositoryManager>();
            Handler = new SpecFunHandler(MockDbManager.Object);
        }

        private SpecialFunction GetSpecFun()
        {
            var specFun = new SpecialFunction(1, "spec_cast_adept");
            SpecFunRepository.Add(specFun.ID, specFun);
            return specFun;
        }

        [Test]
        public void GetSpecFunReference_NoMatch_Test()
        {
            Assert.That(SpecFunHandler.GetSpecFunReference("invalid"), Is.Null);
        }

        [TestCase("spec_cast_adept", true)]
        [TestCase("invalid", false)]
        public void IsValidSpecFunTest(string specFun, bool expectedValue)
        {
            var expectedSpecFun = GetSpecFun();
            MockDbManager.Setup(x => x.GetEntity<SpecialFunction>(It.IsAny<string>())).Returns(expectedSpecFun);

            Assert.That(Handler.IsValidSpecFun(specFun), Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetSpecFun_Valid_Test()
        {
            var expectedSpecFun = GetSpecFun();
            MockDbManager.Setup(x => x.GetEntity<SpecialFunction>(It.IsAny<string>())).Returns(expectedSpecFun);

            Assert.That(Handler.GetSpecFun("spec_cast_adept"), Is.EqualTo(expectedSpecFun));
        }

        [Test]
        public void GetSpecFun_Invalid_Test()
        {
            GetSpecFun();
            MockDbManager.Setup(x => x.GetEntity<SpecialFunction>(It.IsAny<string>())).Returns((SpecialFunction)null);

            Assert.That(Handler.GetSpecFun("spec_cast_cleric"), Is.Null);
        }
    }
}
