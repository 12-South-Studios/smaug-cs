using Realm.Library.Patterns.Repository;
using SmaugCS.Data;

namespace SmaugCS.Repositories
{
    public class AreaRepository : Repository<long, AreaData>
    {
        private AreaData LastArea { get; set; }
    }
}
