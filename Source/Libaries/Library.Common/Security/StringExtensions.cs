using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace Library.Common.Security;

/// <summary>
///
/// </summary>
/// <remarks>Obtained from <a href="http://www.sortedbits.com/string-extension-methods-for-c-2/">http://www.sortedbits.com/string-extension-methods-for-c-2/</a></remarks>
public static class StringExtensions
{
  /// <summary>
  ///
  /// </summary>
  /// <param name="text"></param>
  /// <param name="enc"></param>
  /// <returns></returns>
  [Obsolete("SHA1 is obsolete")]
  public static string ToSha1(this string text, Encoding enc)
  {
    Validation.IsNotNullOrEmpty(text, "text");
    return BitConverter.ToString(SHA1.HashData(enc.GetBytes(text))).Replace("-", "");
  }

  /// <summary>
  ///
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public static string ToMd5(this string input)
  {
    Validation.IsNotNullOrEmpty(input, "input");
    byte[] hash = MD5.HashData(Encoding.ASCII.GetBytes(input));

    StringBuilder sb = new();
    foreach (byte t in hash)
      sb.Append(t.ToString("X2"));
    return sb.ToString();
  }

  /// <summary>
  ///
  /// </summary>
  /// <param name="message"></param>
  /// <param name="passphrase"></param>
  /// <returns></returns>
  [SuppressMessage("Microsoft.Reliability", "CA2000")]
  [Obsolete("MD5 is obsolete")]
  public static string Encrypt(this string message, string passphrase = "")
  {
    Validation.IsNotNullOrEmpty(message, "Message");

    UTF8Encoding utf8 = new();
    byte[] tdesKey = MD5.HashData(utf8.GetBytes(passphrase));

    using TripleDESCryptoServiceProvider tdesAlgorithm = new();
    tdesAlgorithm.Key = tdesKey;
    tdesAlgorithm.Mode = CipherMode.ECB;
    tdesAlgorithm.Padding = PaddingMode.PKCS7;
    byte[] dataToEncrypt = utf8.GetBytes(message);

    byte[] results;
    using (ICryptoTransform encryptor = tdesAlgorithm.CreateEncryptor())
    {
      results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
    }

    return Convert.ToBase64String(results);
  }

  /// <summary>
  ///
  /// </summary>
  /// <param name="message"></param>
  /// <param name="passphrase"></param>
  /// <returns></returns>
  [SuppressMessage("Microsoft.Design", "CA1031")]
  [SuppressMessage("Microsoft.Reliability", "CA2000")]
  [Obsolete("MD5 is obsolete")]
  public static string Decrypt(this string message, string passphrase = "")
  {
    Validation.IsNotNullOrEmpty(message, "Message");

    UTF8Encoding utf8 = new();

    using MD5CryptoServiceProvider hashProvider = new();
    byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(passphrase));

    using TripleDESCryptoServiceProvider tdesAlgorithm = new();
    tdesAlgorithm.Key = tdesKey;
    tdesAlgorithm.Mode = CipherMode.ECB;
    tdesAlgorithm.Padding = PaddingMode.PKCS7;
    byte[] dataToDecrypt = Convert.FromBase64String(message);

    byte[] results;
    using (ICryptoTransform decryptor = tdesAlgorithm.CreateDecryptor())
    {
      results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
    }

    tdesAlgorithm.Clear();
    hashProvider.Clear();

    return utf8.GetString(results);
  }
}