using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;

namespace SmaugCS.Data.Interfaces
{
    public interface IInstanceRepository<out T> where T : Instance
    {
        T Create(Template parent, params object[] args);
    }
}
