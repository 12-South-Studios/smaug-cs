﻿using System.IO;
using Xunit;

namespace Library.Common.Tests;

public class TextWriterProxyTests
{
  [Fact]
  public void WriteTest()
  {
    TextWriterProxy proxy = new(new StringWriter());

    proxy.Write("test");
  }

  [Fact]
  public void Write_1Arg_Test()
  {
    TextWriterProxy proxy = new(new StringWriter());

    proxy.Write("{0} test", 1);
  }

  [Fact]
  public void Write_2Args_Test()
  {
    TextWriterProxy proxy = new(new StringWriter());

    proxy.Write("{0} {1} tests", 2, "big");
  }

  [Fact]
  public void Write_3Args_Test()
  {
    TextWriterProxy proxy = new(new StringWriter());

    proxy.Write("{0} {1} {2} tests", 3, "super", "huge");
  }

  [Fact]
  public void Write_ArrayArgs_Test()
  {
    TextWriterProxy proxy = new(new StringWriter());

    proxy.Write("{0} {1} {2} tests of {3} {4}", [4, "super", "huge", "an", "array"]);
  }
}