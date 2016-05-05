using SmaugCS.Data.Templates;

// ReSharper disable CheckNamespace


namespace SmaugCS.Data
// ReSharper restore CheckNamespace
{
    public interface ITemplateRepository<out T> where T : Template
    {
        T Create(long id, long cloneId, string name);
        T Create(long id, string name);
    }
}
