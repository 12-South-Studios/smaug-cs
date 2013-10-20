using Realm.Library.Common.Logging;
using Realm.Library.Common.Properties;

namespace Realm.Library.Common.Data
{
    /// <summary>
    /// Class that defines a string Atom
    /// </summary>
    public class StringAtom : Atom
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public StringAtom(string value)
            : base(AtomType.String)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the value of the atom
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Dumps the contents of the Atom with the given prefix
        /// </summary>
        /// <param name="log"></param>
        /// <param name="prefix"></param>
        public override void Dump(ILogWrapper log, string prefix)
        {
            Validation.IsNotNull(log, "log");

            log.InfoFormat(Resources.LOG_STRING_ATOM_FORMAT, prefix, Value);
        }

        /// <summary>
        /// Overload of Equals to compare two objects
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var stringAtom = obj as StringAtom;
            return stringAtom != null && Value.Equals(stringAtom.Value);
        }

        /// <summary>
        /// Overload of GetHashCode to provide a different hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}