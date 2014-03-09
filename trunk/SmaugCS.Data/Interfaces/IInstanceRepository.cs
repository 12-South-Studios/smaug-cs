// ReSharper disable CheckNamespace
namespace SmaugCS.Data
// ReSharper restore CheckNamespace
{
    public interface IInstanceRepository<out T> where T : Instance
    {
        T Create(Template parent, params object[] args);
    }
}
