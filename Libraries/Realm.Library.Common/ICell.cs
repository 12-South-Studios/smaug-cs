namespace Realm.Library.Common
{
    /// <summary>
    /// Basic interface for the Cell object
    /// </summary>
    public interface ICell
    {
        /// <summary>
        /// Gets the ID of the Cell
        /// </summary>
        long ID { get; }

        /// <summary>
        /// Gets the Name of the Cell
        /// </summary>
        string Name { get; }
    }
}