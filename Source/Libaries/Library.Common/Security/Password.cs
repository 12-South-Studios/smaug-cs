using System;
using System.Security.Cryptography;

namespace Library.Common.Security;

/// <summary>
/// Utility class that contains password related functions
/// </summary>
public static class Password
{
  #region Version 0

  /// <summary>
  ///
  /// </summary>
  [Obsolete("SHA1 is obsolete")]
  public static string ComputeHashV0(PasswordRequestv0 request)
  {
    Validation.IsNotNull(request, "request");
    byte[] p1 = Convert.FromBase64String(request.HashSalt);
    byte[] p2 = System.Text.Encoding.Unicode.GetBytes(request.PlainPassword);

    byte[] data = new byte[p1.Length + p2.Length];

    p1.CopyTo(data, 0);
    p2.CopyTo(data, p1.Length);
    return Convert.ToBase64String(SHA1.HashData(data));
  }

  [Obsolete("SHA1 is obsolete")]
  private static string GeneratePasswordHashV0(PasswordRequestv0 request)
  {
    Validation.IsNotNull(request, "request");

    string hashSalt = GenerateRandomToken();
    return $"{ComputeHashV0(request)}:{hashSalt}";
  }

  #endregion Version 0

  #region Version 1

  /// <summary>
  /// Validates a password using two hash values
  /// </summary>
  [Obsolete("SHA1 is obsolete")]
  public static bool ValidatePasswordHashV1(PasswordRequestv1 request)
  {
    Validation.IsNotNull(request, "request");

    if (request.PlainPassword == request.PreHash || request.PlainPassword == request.PostHash) return false;
    return ValidatePasswordV2(request);
  }

  /// <summary>
  ///
  /// </summary>
  [Obsolete("SHA1 is obsolete")]
  public static string ComputeHashV1(PasswordRequestv1 request)
  {
    Validation.IsNotNull(request, "request");
    byte[] p1 = Convert.FromBase64String(request.PreHash);
    byte[] p2 = System.Text.Encoding.Unicode.GetBytes(request.PlainPassword);
    byte[] p3 = Convert.FromBase64String(request.PostHash);

    byte[] data = new byte[p1.Length + p2.Length + p3.Length];

    p1.CopyTo(data, 0);
    p2.CopyTo(data, p1.Length);
    p3.CopyTo(data, p3.Length);
    return Convert.ToBase64String(SHA1.HashData(data));
  }

  private static string GeneratePasswordHashV1(PasswordRequestv1 request)
  {
    Validation.IsNotNull(request, "request");

    return request.PlainPassword;
  }

  [Obsolete("SHA1 is obsolete")]
  private static bool ValidatePasswordV2(PasswordRequestv1 request)
  {
    Validation.IsNotNull(request, "request");

    return request.HashedPassword.Equals(ComputeHashV1(request));
  }

  #endregion Version 1

  /// <summary>
  /// Generates a password hash (:v0 or :v1)
  /// </summary>
  [Obsolete("SHA1 is obsolete")]
  public static string GeneratePasswordHash(string version, string plainPassword)
  {
    Validation.IsNotNullOrEmpty(plainPassword, "plainPassword");
    Validation.IsNotNullOrEmpty(version, "version");

    string hash = version switch
    {
      "v0" => GeneratePasswordHashV0(new PasswordRequestv0 { PlainPassword = plainPassword }),
      "v1" => GeneratePasswordHashV1(new PasswordRequestv1 { PlainPassword = plainPassword }),
      _ => string.Empty
    };
    return $"{version}:{hash}";
  }

  /// <summary>
  /// Generates a random token using a RNGCryptoServiceProvider
  /// </summary>
  [Obsolete("SHA1 is obsolete")]
  public static string GenerateRandomToken()
  {
    byte[] hashSaltBytes = new byte[16];
    using RNGCryptoServiceProvider rng = new();
    rng.GetBytes(hashSaltBytes);
    return Convert.ToBase64String(hashSaltBytes);
  }

  /// <summary>
  /// Generates a 16-bit hex token using a RNGCryptoServiceProvider
  /// </summary>
  [Obsolete("SHA1 is obsolete")]
  public static string GenerateRandomHexToken()
  {
    byte[] hashSaltBytes = new byte[16];
    using RNGCryptoServiceProvider rng = new();
    rng.GetBytes(hashSaltBytes);
    return BitConverter.ToString(hashSaltBytes).Replace("-", string.Empty);
  }
}