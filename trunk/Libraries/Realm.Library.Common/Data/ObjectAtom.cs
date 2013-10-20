using Realm.Library.Common.Logging;
using Realm.Library.Common.Properties;

namespace Realm.Library.Common.Data
{
    /// <summary>
    /// Class that defines an object Atom
    /// </summary>
    public class ObjectAtom : Atom
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public ObjectAtom(object value)
            : base(AtomType.Object)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the value of the atom
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Dumps the contents of the Atom with the given prefix
        /// </summary>
        /// <param name="log"></param>
        /// <param name="prefix"></param>
        public override void Dump(ILogWrapper log, string prefix)
        {
            Validation.IsNotNull(log, "log");

            log.InfoFormat(Resources.LOG_OBJECT_ATOM_FORMAT, prefix, Value);
        }
    }
}