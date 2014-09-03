
// ReSharper disable once CheckNamespace
namespace SmaugCS.Data
{
    public interface IInstanceRepository<T> where T : Instance
    {
        T Create(Template parent, params object[] args);
        T Clone(T source, params object[] args);
    }
}
