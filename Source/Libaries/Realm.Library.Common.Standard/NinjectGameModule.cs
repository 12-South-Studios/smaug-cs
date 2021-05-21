using Ninject.Modules;
using Realm.Library.Common.Data;
using Realm.Library.Common.Events;

namespace Realm.Library.Common
{
    public abstract class NinjectGameModule : NinjectModule
    {
        protected DictionaryAtom InitAtom { get; private set; }
        protected BooleanSet InitBooleanSet { get; private set; }

        protected NinjectGameModule(DictionaryAtom initAtom, BooleanSet initBooleanSet)
        {
            InitAtom = initAtom;
            InitBooleanSet = initBooleanSet;
        }
    }
}
