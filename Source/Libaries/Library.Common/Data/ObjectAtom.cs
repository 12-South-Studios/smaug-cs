using Library.Common.Logging;

namespace Library.Common.Data;

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
  public object Value { get; }

  /// <summary>
  /// Dumps the contents of the Atom with the given prefix
  /// </summary>
  /// <param name="log"></param>
  /// <param name="prefix"></param>
  public override void Dump(ILogWrapper log, string prefix)
  {
    Validation.IsNotNull(log, "log");

    log.Info("{0}:{1}(ObjectAtom)", prefix, Value);
  }
}