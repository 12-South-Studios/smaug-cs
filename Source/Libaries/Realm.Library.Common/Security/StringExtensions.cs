using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace Realm.Library.Common.Security
{
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
        public static string ToSHA1(this string text, Encoding enc)
        {
            Validation.IsNotNullOrEmpty(text, "text");

            using (var cryptoTransformSha1 = new SHA1CryptoServiceProvider())
            {
                return BitConverter.ToString(cryptoTransformSha1.ComputeHash(enc.GetBytes(text))).Replace("-", "");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToMD5(this string input)
        {
            Validation.IsNotNullOrEmpty(input, "input");

            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(input));

                var sb = new StringBuilder();
                foreach (byte t in hash)
                    sb.Append(t.ToString("X2"));
                return sb.ToString();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000")]
        public static string Encrypt(this string message, string passphrase = "")
        {
            Validation.IsNotNullOrEmpty(message, "Message");

            var utf8 = new UTF8Encoding();

            using (var hashProvider = new MD5CryptoServiceProvider())
            {
                var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(passphrase));

                using (var tdesAlgorithm = new TripleDESCryptoServiceProvider
                    {
                        Key = tdesKey,
                        Mode = CipherMode.ECB,
                        Padding = PaddingMode.PKCS7
                    })
                {
                    var dataToEncrypt = utf8.GetBytes(message);

                    byte[] results;
                    using (var encryptor = tdesAlgorithm.CreateEncryptor())
                    {
                        results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
                    }

                    return Convert.ToBase64String(results);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1031")]
        [SuppressMessage("Microsoft.Reliability", "CA2000")]
        public static string Decrypt(this string message, string passphrase = "")
        {
            Validation.IsNotNullOrEmpty(message, "Message");

            var utf8 = new UTF8Encoding();

            using (var hashProvider = new MD5CryptoServiceProvider())
            {
                var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(passphrase));

                using (var tdesAlgorithm = new TripleDESCryptoServiceProvider
                    {
                        Key = tdesKey,
                        Mode = CipherMode.ECB,
                        Padding = PaddingMode.PKCS7
                    })
                {
                    var dataToDecrypt = Convert.FromBase64String(message);

                    byte[] results;
                    using (var decryptor = tdesAlgorithm.CreateDecryptor())
                    {
                        results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
                    }

                    tdesAlgorithm.Clear();
                    hashProvider.Clear();

                    return utf8.GetString(results);
                }
            }
        }
    }
}