using FluentAssertions;
using Patterns.Repository;
using System.Collections.Generic;
using Xunit;

namespace Patterns.Tests;

public class RepositoryTests
{
  private class TestRepository : Repository<string, object>;

  [Fact]
  public void Repository_Contains_Test()
  {
    TestRepository repository = new();
    repository.Add("Test", new object());

    bool result = repository.Contains("Test");
    result.Should().BeTrue();
  }

  [Fact]
  public void Repository_AddNoMatch_Test()
  {
    TestRepository repository = new();

    bool result = repository.Add("Test", new object());
    result.Should().BeTrue();
  }

  [Fact]
  public void Repository_AddMatch_Test()
  {
    TestRepository repository = new();
    repository.Add("Test", new object());

    bool result = repository.Add("Test", new object());
    result.Should().BeFalse();
  }

  [Fact]
  public void Repository_DeleteMatch_Test()
  {
    TestRepository repository = new();
    repository.Add("Test", new object());

    bool result = repository.Delete("Test");
    result.Should().BeTrue();
  }

  [Fact]
  public void Repository_DeleteNoMatch_Test()
  {
    TestRepository repository = new();
    repository.Add("Test", new object());

    bool result = repository.Delete("Tester");
    result.Should().BeFalse();
  }

  [Fact]
  public void Repository_GetMatch_Test()
  {
    TestRepository repository = new();
    repository.Add("Test", new object());

    object result = repository.Get("Test");
    result.Should().NotBeNull();
  }

  [Fact]
  public void Repository_GetNoMatch_Test()
  {
    TestRepository repository = new();
    repository.Add("Test", new object());

    object result = repository.Get("tester");
    result.Should().BeNull();
  }

  [Fact]
  public void Repository_Clear_Test()
  {
    TestRepository repository = new();
    repository.Add("Test", new object());
    repository.Clear();

    repository.Count.Should().Be(0);
  }

  [Fact]
  public void Repository_Count_Test()
  {
    TestRepository repository = new();
    repository.Add("Test", new object());
    repository.Add("Tester", new object());

    repository.Count.Should().Be(2);
  }

  [Fact]
  public void Repository_Keys_Test()
  {
    TestRepository repository = new();
    repository.Add("Test", new object());
    repository.Add("Tester", new object());

    List<string> keyList = [..repository.Keys];
    keyList.Count.Should().Be(2);
  }

  [Fact]
  public void RepositoryValuesTest()
  {
    TestRepository repository = new();
    repository.Add("Test", new object());
    repository.Add("Tester", new object());

    List<object> valueList = [..repository.Values];
    valueList.Count.Should().Be(2);
  }
}