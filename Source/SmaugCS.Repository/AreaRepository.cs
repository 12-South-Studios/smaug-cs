using Realm.Standard.Patterns.Repository;
using SmaugCS.Data;

namespace SmaugCS.Repository
{
    public class AreaRepository : Repository<long, AreaData>
    {
        private AreaData LastArea { get; set; }
    }
}
