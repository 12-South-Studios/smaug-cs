
namespace Realm.Library.Common.Events

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