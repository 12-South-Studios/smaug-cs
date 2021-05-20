
namespace Realm.Library.Common.Entities

{
    /// <summary>
    /// Basic entity module contract
    /// </summary>
    public interface IEntityModule
    {
        /// <summary>
        /// Owner of the module
        /// </summary>
        IEntity Owner { get; }
    }
}