using Realm.Library.Common.Objects;

namespace SmaugCS.Managers
{
    public sealed class LookupManager : GameSingleton
    {
        private static LookupManager _instance;
        private static readonly object Padlock = new object();

        private LookupManager()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public static LookupManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new LookupManager());
                }
            }
        }
    }
}
