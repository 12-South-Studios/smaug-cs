namespace Realm.Library.Common.Security
{
    /// <summary>
    ///
    /// </summary>
    public class PasswordRequestv1
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
        public string PreHash { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string PostHash { get; set; }
    }
}