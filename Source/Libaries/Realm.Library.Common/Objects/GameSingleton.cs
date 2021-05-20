using Realm.Library.Common.Data;
using Realm.Library.Common.Events;
using Realm.Library.Common.Logging;
using Realm.Library.Common.Properties;
using Realm.Standard.Patterns.Singleton;

namespace Realm.Library.Common.Objects

{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GameSingleton : Singleton, IGameSingleton
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public virtual void Instance_OnGameInitialize(RealmEventArgs args)
        {
            var booleanSet = args.GetValue("BooleanSet") as BooleanSet;
            if (booleanSet.IsNull()) return;

            booleanSet.CompleteItem(GetType().Name);

            var initAtom = args.GetValue("InitAtom") as DictionaryAtom;
            var logger = initAtom.GetObject("Logger").CastAs<LogWrapper>();
            logger.InfoFormat(Resources.MSG_INITIALIZE_MANAGER, GetType());
        }
    }
}
