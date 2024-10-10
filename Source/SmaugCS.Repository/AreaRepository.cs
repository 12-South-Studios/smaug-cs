using SmaugCS.Data;

namespace SmaugCS.Repository;

public class AreaRepository : Patterns.Repository.Repository<long, AreaData>
{
    private AreaData LastArea { get; set; }
}