using Realm.Library.Common.Logging;
using Realm.Library.Common.Properties;

namespace Realm.Library.Common.Data
{
    public class BoolAtom : Atom
    {
        public BoolAtom(bool value)
            : base(AtomType.Boolean)
        {
            Value = value;
        }

        public bool Value { get; private set; }

        public override void Dump(ILogWrapper log, string prefix)
        {
            Validation.IsNotNull(log, "log");

            log.InfoFormat(Resources.LOG_BOOL_ATOM_FORMAT, prefix, Value);
        }
    }
}