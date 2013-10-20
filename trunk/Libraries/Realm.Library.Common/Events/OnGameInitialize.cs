// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Core event type used when the game is initialized
    /// </summary>
    public sealed class OnGameInitialize : EventBase
    {
        /// <summary>
        ///
        /// </summary>
        public OnGameInitialize()
        {
            Name = "OnGameInitialize";
        }
    }
}