using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;

namespace SmaugCS.DAL.Interfaces
{
    public interface ISmaugContext
    {
        ObjectContext ObjectContext { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
