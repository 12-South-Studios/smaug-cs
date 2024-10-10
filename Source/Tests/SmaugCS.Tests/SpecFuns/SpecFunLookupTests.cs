using FakeItEasy;
using FluentAssertions;
using SmaugCS.Data;
using SmaugCS.Repository;
using SmaugCS.SpecFuns;
using Xunit;

namespace SmaugCS.Tests.SpecFuns;

public class SpecFunLookupTests
{
  private readonly GenericRepository<SpecialFunction> _specFunRepository;
  private readonly IRepositoryManager _mockDbManager;
  private readonly SpecFunHandler _handler;

  public SpecFunLookupTests()
  {
    _specFunRepository = new GenericRepository<SpecialFunction>();
    _mockDbManager = A.Fake<IRepositoryManager>();
    _handler = new SpecFunHandler(_mockDbManager);
  }

  private SpecialFunction GetSpecFun()
  {
    SpecialFunction specFun = new(1, "spec_cast_adept");
    _specFunRepository.Add(specFun.Id, specFun);
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
    SpecialFunction expectedSpecFun = GetSpecFun();
    A.CallTo(() => _mockDbManager.GetEntity<SpecialFunction>(A<string>.Ignored)).Returns(expectedSpecFun);

    _handler.IsValidSpecFun(specFun).Should().Be(expectedValue);
  }

  [Fact]
  public void GetSpecFun_Valid_Test()
  {
    SpecialFunction expectedSpecFun = GetSpecFun();
    A.CallTo(() => _mockDbManager.GetEntity<SpecialFunction>(A<string>.Ignored)).Returns(expectedSpecFun);

    _handler.GetSpecFun("spec_cast_adept").Should().Be(expectedSpecFun);
  }

  [Fact]
  public void GetSpecFun_Invalid_Test()
  {
    GetSpecFun();
    A.CallTo(() => _mockDbManager.GetEntity<SpecialFunction>(A<string>.Ignored)).Returns((SpecialFunction)null);

    _handler.GetSpecFun("spec_cast_cleric").Should().BeNull();
  }
}