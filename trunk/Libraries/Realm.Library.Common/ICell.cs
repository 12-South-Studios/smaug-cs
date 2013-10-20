// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Basic interface for the Cell object
    /// </summary>
    public interface ICell
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Unique identifier of the object
        /// </summary>
        long ID { get; }

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Unique name of the object
        /// </summary>
        string Name { get; }
    }
}