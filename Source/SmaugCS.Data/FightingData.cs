using SmaugCS.Data.Instances;

namespace SmaugCS.Data;

public class FightingData
{
    public CharacterInstance Who { get; set; }
    public int Experience { get; set; }
    public int Alignment { get; set; }
    public int Duration { get; set; }
    public int TimesKilled { get; set; }
}