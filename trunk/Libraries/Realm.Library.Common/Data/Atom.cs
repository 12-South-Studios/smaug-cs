using Realm.Library.Common.Logging;

namespace Realm.Library.Common.Data
{
    /// <summary>
    /// Abstract class that defines an Atom
    /// </summary>
    public abstract class Atom
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        protected Atom(AtomType type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets the type of Atom
        /// </summary>
        public AtomType Type { get; private set; }

        /// <summary>
        /// Dumps the contents of the Atom with the given prefix
        /// </summary>
        /// <param name="log"></param>
        /// <param name="prefix"></param>
        public abstract void Dump(ILogWrapper log, string prefix);
    }
}