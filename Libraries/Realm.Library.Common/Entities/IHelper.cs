
// ReSharper disable once CheckNamespace
namespace Realm.Library.Common
{
    public interface IHelper<out T>
    {
        T Get(string key);
    }
}