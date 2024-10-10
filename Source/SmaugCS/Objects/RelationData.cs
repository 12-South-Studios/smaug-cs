using SmaugCS.Constants.Enums;

namespace SmaugCS;

public class RelationData
{
    public object Actor { get; set; }
    public object Subject { get; set; }
    public RelationTypes Types { get; set; }
}