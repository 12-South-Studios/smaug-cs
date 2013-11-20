namespace Realm.Library.Common
{
    /// <summary>
    /// Basic interface for the Cell object
    /// </summary>
    public interface ICell
    {
        long ID { get; }
        string Name { get; }
    }
}