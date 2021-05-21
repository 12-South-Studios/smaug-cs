
namespace Realm.Library.Common.Entities

{
    /// <summary>
    ///
    /// </summary>
    public interface IHelper<out T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get(string key);
    }
}