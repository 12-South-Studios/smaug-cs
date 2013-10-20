// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
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