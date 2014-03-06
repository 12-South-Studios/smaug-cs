using Moq;
using NUnit.Framework;
using SmaugCS.Data;
using SmaugCS.Managers;
using SmaugCS.Repositories;
using SmaugCS.SpecFuns;

namespace SmaugCS.Tests.SpecFuns
{
    [TestFixture]
    public class SpecFunLookupTests
    {
        [Test]
        public void GetSpecFunReference_NoMatch_Test()
        {
            var actual = SpecFunHandler.GetSpecFunReference("invalid");

            Assert.That(actual, Is.Null);
        }

        [TestCase("spec_cast_adept", true)]
        [TestCase("invalid", false)]
        public void IsValidSpecFunTest(string specFun, bool expectedValue)
        {
            var expectedSpecFun = new SpecialFunction(1, "spec_cast_adept");

            var repo = new GenericRepository<SpecialFunction>();
            repo.Add(expectedSpecFun.ID, expectedSpecFun);

            var mockDbManager = new Mock<IDatabaseManager>();
            mockDbManager.Setup(x => x.GetEntity<SpecialFunction>(It.IsAny<string>())).Returns(expectedSpecFun);

            var handler = new SpecFunHandler(mockDbManager.Object);

            Assert.That(handler.IsValidSpecFun(specFun), Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetSpecFun_Valid_Test()
        {
            var expectedSpecFun = new SpecialFunction(1, "spec_cast_adept");

            var repo = new GenericRepository<SpecialFunction>();
            repo.Add(expectedSpecFun.ID, expectedSpecFun);

            var mockDbManager = new Mock<IDatabaseManager>();
            mockDbManager.Setup(x => x.GetEntity<SpecialFunction>(It.IsAny<string>())).Returns(expectedSpecFun);

            var handler = new SpecFunHandler(mockDbManager.Object);

            Assert.That(handler.GetSpecFun("spec_cast_adept"), Is.EqualTo(expectedSpecFun));
        }

        [Test]
        public void GetSpecFun_Invalid_Test()
        {
            var expectedSpecFun = new SpecialFunction(1, "spec_cast_adept");

            var repo = new GenericRepository<SpecialFunction>();
            repo.Add(expectedSpecFun.ID, expectedSpecFun);

            var mockDbManager = new Mock<IDatabaseManager>();
            mockDbManager.Setup(x => x.GetEntity<SpecialFunction>(It.IsAny<string>())).Returns((SpecialFunction) null);

            var handler = new SpecFunHandler(mockDbManager.Object);

            Assert.That(handler.GetSpecFun("spec_cast_cleric"), Is.Null);
        }
    }
}
