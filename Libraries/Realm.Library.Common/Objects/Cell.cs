// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Atomic abstract object class containing an ID and a Name.
    /// </summary>
    public abstract class Cell : ICell
    {
        /// <summary>
        /// Gets or sets the long ID of the Cell
        /// </summary>
        public long ID { get; protected set; }

        /// <summary>
        /// Gets or sets the string Name of the Cell
        /// </summary>
        public string Name { get; protected set; }
    }
}