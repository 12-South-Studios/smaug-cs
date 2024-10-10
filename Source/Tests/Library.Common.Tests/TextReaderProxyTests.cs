using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using FluentAssertions.Execution;
using Test.Common;
using Xunit;

namespace Library.Common.Tests;

[Collection(CollectionDefinitions.NonParallelCollection)]
public class TextReaderProxyTests
{
  [Fact]
  public void ReadNextLetter_ReturnsValidChar()
  {
    TextReaderProxy proxy = new(new StringReader("a word"));

    char result = proxy.ReadNextLetter();
    result.Should().Be('a');
  }

  [Fact]
  public void ReadNextLetter_ThrowsExceptionAtEndOfStream()
  {
    TextReaderProxy proxy = new(new StringReader(""));

    Action act = () => proxy.ReadNextLetter();
    act.Should().Throw<IOException>();
  }

  [Fact]
  public void ReadNextWord_ReturnsValidString()
  {
    TextReaderProxy proxy = new(new StringReader("first word"));

    string result = proxy.ReadNextWord();
    result.Should().Be("first");
  }

  [Fact]
  public void ReadNumber_ReturnsValidValue()
  {
    TextReaderProxy proxy = new(new StringReader("100 words"));

    int result = proxy.ReadNumber();
    result.Should().Be(100);
  }

  [Fact]
  public void ReadIntoList_ReturnsValidList()
  {
    TextReaderProxy proxy = new(new StringReader("#common|~|abcdefghijklmnopqrstuvwxyz~|~|"));

    List<string> results = proxy.ReadIntoList(['|']);

    using (new AssertionScope())
    {
      results.Should().NotBeNull();
      results.Count.Should().Be(4);
      results[0].Should().Be("#common");
      results[3].Should().Be("~");
    }
  }

  [Fact]
  public void ReadSections_HasInvalidSectionParameters()
  {
    TextReaderProxy proxy =
      new(new StringReader(
        "#common\r\n~\r\nabcdefghijklmnopqrstuvwxyz~\r\n~\n\r#uncommon\r\n~\r\nabcdefghijklmnopqrstuvwxyz~\r\n~\n\r#end\n\r"));

    List<TextSection> results = proxy.ReadSections(new List<string> { "#" }, new List<string> { "*" },
      new List<string> { "#" }, "#end");

    results.Count.Should().Be(0);
  }

  [Fact]
  public void ReadSections_HasFooter()
  {
    TextReaderProxy proxy =
      new(new StringReader(
        "#common\r\n~\r\nabcdefghijklmnopqrstuvwxyz~\r\n~\r\n^end_common\n\r#uncommon\r\n~\r\nabcdefghijklmnopqrstuvwxyz~\r\n~\n\r^end_uncommon\r\n#end\n\r"));

    List<TextSection> results = proxy.ReadSections(new List<string> { "#" }, 
      new List<string> { "*" }, new List<string> { "^" }, "#end");

    results.Should().NotBeNull();
    results[0].Footer.Should().Be("end_common");
  }

  [Fact]
  public void ReadSections_ReturnsValidList()
  {
    TextReaderProxy proxy =
      new(new StringReader("#common|~|abcdefghijklmnopqrstuvwxyz~|~|#uncommon|~|abcdefghijklmnopqrstuvwxyz~|~|#end|"));

    List<TextSection> results = proxy.ReadSections(new List<string> { "#" }, 
      new List<string> { "*" }, null, "#end", ['|']);

    using (new AssertionScope())
    {
      results.Should().NotBeNull();
      results.Count.Should().Be(2);
      results[0].Header.Should().Be("common");
      results[0].Lines.Count.Should().Be(3);
    }
  }

  [Fact]
  public void ReadEndOfLine_DoesntReadNextLine()
  {
    TextReaderProxy proxy =
      new(new StringReader("This is a test\n\rwith two lines."));

    char result1 = proxy.ReadNextLetter();
    result1.Should().Be('T');

    string result2 = proxy.ReadToEndOfLine(true);
    result2.Should().Be("his is a test");

    string result3 = proxy.ReadLine();
    result3.Should().Be("with two lines.");
  }
}