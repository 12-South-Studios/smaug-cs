using System;
using System.Security.Cryptography;

namespace Realm.Library.Common.Security
{
    /// <summary>
    /// Utility class that contains password related functions
    /// </summary>
    public static class Password
    {
        #region Version 0

        /// <summary>
        ///
        /// </summary>
        public static string ComputeHashV0(PasswordRequestv0 request)
        {
            Validation.IsNotNull(request, "request");

            using (SHA1 sha = new SHA1CryptoServiceProvider())
            {
                var p1 = Convert.FromBase64String(request.HashSalt);
                var p2 = System.Text.Encoding.Unicode.GetBytes(request.PlainPassword);

                var data = new byte[p1.Length + p2.Length];

                p1.CopyTo(data, 0);
                p2.CopyTo(data, p1.Length);
                return Convert.ToBase64String(sha.ComputeHash(data));
            }
        }

        private static string GeneratePasswordHashV0(PasswordRequestv0 request)
        {
            Validation.IsNotNull(request, "request");

            var hashSalt = GenerateRandomToken();
            return String.Format("{0}:{1}", ComputeHashV0(request), hashSalt);
        }

        #endregion Version 0

        #region Version 1

        /// <summary>
        /// Validates a password using two hash values
        /// </summary>
        public static bool ValidatePasswordHashV1(PasswordRequestv1 request)
        {
            Validation.IsNotNull(request, "request");

            if (request.PlainPassword == request.PreHash || request.PlainPassword == request.PostHash) return false;
            return ValidatePasswordV2(request);
        }

        /// <summary>
        ///
        /// </summary>
        public static string ComputeHashV1(PasswordRequestv1 request)
        {
            Validation.IsNotNull(request, "request");

            using (SHA1 sha = new SHA1CryptoServiceProvider())
            {
                var p1 = Convert.FromBase64String(request.PreHash);
                var p2 = System.Text.Encoding.Unicode.GetBytes(request.PlainPassword);
                var p3 = Convert.FromBase64String(request.PostHash);

                var data = new byte[p1.Length + p2.Length + p3.Length];

                p1.CopyTo(data, 0);
                p2.CopyTo(data, p1.Length);
                p3.CopyTo(data, p3.Length);
                return Convert.ToBase64String(sha.ComputeHash(data));
            }
        }

        private static string GeneratePasswordHashV1(PasswordRequestv1 request)
        {
            Validation.IsNotNull(request, "request");

            return request.PlainPassword;
        }

        private static bool ValidatePasswordV2(PasswordRequestv1 request)
        {
            Validation.IsNotNull(request, "request");

            return request.HashedPassword.Equals(ComputeHashV1(request));
        }

        #endregion Version 1

        /// <summary>
        /// Generates a password hash (:v0 or :v1)
        /// </summary>
        public static string GeneratePasswordHash(string version, string plainPassword)
        {
            Validation.IsNotNullOrEmpty(plainPassword, "plainPassword");
            Validation.IsNotNullOrEmpty(version, "version");

            var hash = string.Empty;
            if (version.Equals("v0"))
                hash = GeneratePasswordHashV0(new PasswordRequestv0 { PlainPassword = plainPassword });
            else if (version.Equals("v1"))
                hash = GeneratePasswordHashV1(new PasswordRequestv1 { PlainPassword = plainPassword });
            return String.Format("{0}:{1}", version, hash);
        }

        /// <summary>
        /// Generates a random token using a RNGCryptoServiceProvider
        /// </summary>
        public static string GenerateRandomToken()
        {
            var hashSaltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(hashSaltBytes);
                return Convert.ToBase64String(hashSaltBytes);
            }
        }

        /// <summary>
        /// Generates a 16-bit hex token using a RNGCryptoServiceProvider
        /// </summary>
        public static string GenerateRandomHexToken()
        {
            var hashSaltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(hashSaltBytes);
                return BitConverter.ToString(hashSaltBytes).Replace("-", string.Empty);
            }
        }
    }
}