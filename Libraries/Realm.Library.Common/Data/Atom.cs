using Realm.Library.Common.Logging;

namespace Realm.Library.Common.Data
{
    public abstract class Atom
    {
        protected Atom(AtomType type)
        {
            Type = type;
        }

        public AtomType Type { get; private set; }

        public abstract void Dump(ILogWrapper log, string prefix);
    }
}