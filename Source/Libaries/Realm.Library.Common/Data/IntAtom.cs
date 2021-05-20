using Realm.Library.Common.Logging;
using Realm.Library.Common.Properties;

namespace Realm.Library.Common.Data
{
    /// <summary>
    /// Class that defines an integer Atom
    /// </summary>
    public class IntAtom : Atom
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public IntAtom(int value)
            : base(AtomType.Integer)
        {
            Value = value;
        }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="value"></param>
        public IntAtom(long value)
            : base(AtomType.Integer)
        {
            Value = (int)value;
        }

        /// <summary>
        /// Gets the value of the atom
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Dumps the contents of the Atom with the given prefix
        /// </summary>
        /// <param name="log"></param>
        /// <param name="prefix"></param>
        public override void Dump(ILogWrapper log, string prefix)
        {
            Validation.IsNotNull(log, "log");

            log.InfoFormat(Resources.LOG_INT_ATOM_FORMAT, prefix, Value);
        }

        /// <summary>
        /// Overload of Equals to compare two objects
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var intAtom = obj as IntAtom;
            return intAtom != null && Value == intAtom.Value;
        }

        /// <summary>
        /// Overload of GetHashCode to provide a different hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value;
        }
    }
}