// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Behavior options for how exceptions can be handled
    /// </summary>
    public enum ExceptionHandlingOptions
    {
        /// <summary>
        ///
        /// </summary>
        RecordAndThrow,

        /// <summary>
        ///
        /// </summary>
        RecordOnly,

        /// <summary>
        ///
        /// </summary>
        ThrowOnly,

        /// <summary>
        ///
        /// </summary>
        Suppress
    };
}