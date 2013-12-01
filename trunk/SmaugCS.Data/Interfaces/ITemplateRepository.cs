using SmaugCS.Data.Templates;

namespace SmaugCS.Data.Interfaces
{
    public interface ITemplateRepository<out T> where T : Template
    {
        T Create(long id, long cloneId, string name);
        T Create(long id, string name);
    }
}
