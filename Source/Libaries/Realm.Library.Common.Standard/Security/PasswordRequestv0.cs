namespace Realm.Library.Common.Security
{
    /// <summary>
    ///
    /// </summary>
    public class PasswordRequestv0
    {
        /// <summary>
        ///
        /// </summary>
        public string PlainPassword { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string HashedPassword { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string HashSalt { get; set; }
    }
}