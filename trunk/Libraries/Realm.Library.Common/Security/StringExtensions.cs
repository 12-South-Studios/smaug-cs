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

            using (var cryptoTransformSHA1 = new SHA1CryptoServiceProvider())
            {
                return BitConverter.ToString(cryptoTransformSHA1.ComputeHash(enc.GetBytes(text))).Replace("-", "");
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
                for (int i = 0; i < hash.Length; i++)
                    sb.Append(hash[i].ToString("X2"));
                return sb.ToString();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000")]
        public static string Encrypt(this string Message, string passphrase = "")
        {
            Validation.IsNotNullOrEmpty(Message, "Message");

            var UTF8 = new UTF8Encoding();

            using (var HashProvider = new MD5CryptoServiceProvider())
            {
                var TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));

                using (var TDESAlgorithm = new TripleDESCryptoServiceProvider
                    {
                        Key = TDESKey,
                        Mode = CipherMode.ECB,
                        Padding = PaddingMode.PKCS7
                    })
                {
                    var DataToEncrypt = UTF8.GetBytes(Message);

                    byte[] Results;
                    using (var Encryptor = TDESAlgorithm.CreateEncryptor())
                    {
                        Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
                    }

                    return Convert.ToBase64String(Results);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1031")]
        [SuppressMessage("Microsoft.Reliability", "CA2000")]
        public static string Decrypt(this string Message, string passphrase = "")
        {
            Validation.IsNotNullOrEmpty(Message, "Message");

            var UTF8 = new UTF8Encoding();

            using (var HashProvider = new MD5CryptoServiceProvider())
            {
                var TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));

                using (var TDESAlgorithm = new TripleDESCryptoServiceProvider
                    {
                        Key = TDESKey,
                        Mode = CipherMode.ECB,
                        Padding = PaddingMode.PKCS7
                    })
                {
                    var DataToDecrypt = Convert.FromBase64String(Message);

                    byte[] Results;
                    using (var Decryptor = TDESAlgorithm.CreateDecryptor())
                    {
                        Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                    }

                    TDESAlgorithm.Clear();
                    HashProvider.Clear();

                    return UTF8.GetString(Results);
                }
            }
        }
    }
}