using System.Data.Entity.Core.Objects;

namespace SmaugCS.DAL.Interfaces
{
    public interface ISmaugContext
    {
        ObjectContext ObjectContext { get; }
        int SaveChanges();
    }
}
