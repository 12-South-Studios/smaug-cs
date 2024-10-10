using System;
using FluentAssertions;
using Library.Common.Security;
using Xunit;

namespace Library.Common.Tests;

public class PasswordTest
{
  private static string PreHash => "tqzOngQC";
  private static string PostHash => "LfbPsmik";
  private static string EncryptedPass => "SeFROWqcINCrgwcZ/zYbvKr497A=";

  [Theory]
  [InlineData("12south", true)]
  [InlineData("tester", false)]
  [Obsolete("Obsolete")]
  public void ComputeHashV1Test(string password, bool expected)
  {
    string actual = Password.ComputeHashV1(new PasswordRequestv1
    {
      PlainPassword = password,
      PreHash = PreHash,
      PostHash = PostHash
    });

    EncryptedPass.Equals(actual).Should().Be(expected);
  }

  [Theory]
  [InlineData("12south", true)]
  [InlineData("tester", false)]
  [Obsolete("Obsolete")]
  public void ValidatePasswordHashV1Test(string password, bool expected)
  {
    bool actual = Password.ValidatePasswordHashV1(new PasswordRequestv1
    {
      PlainPassword = password,
      HashedPassword = EncryptedPass,
      PreHash = PreHash,
      PostHash = PostHash
    });

    actual.Should().Be(expected);
  }
}