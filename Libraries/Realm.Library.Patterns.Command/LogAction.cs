namespace Realm.Library.Patterns.Command
{
    /// <summary>
    /// LogAction enumeration which is used to tell the game when to log the execution
    /// of a command-verb (example: Normal, Always, etc)
    /// </summary>
    public enum LogAction
    {
        /// <summary>
        /// Don't ever log, ever
        /// </summary>
        Never,

        /// <summary>
        /// Log only if told to log
        /// </summary>
        Normal,

        /// <summary>
        /// Always log this
        /// </summary>
        Always
    }
}