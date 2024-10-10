using SmaugCS.Constants.Enums;

namespace SmaugCS.Data;

public class MudProgData
{
    public MudProgTypes Type { get; set; }
    public bool triggered { get; set; }
    public int resetdelay { get; set; }
    public string ArgList { get; set; }
    public string Script { get; set; }
    public bool IsFileProg { get; set; }
}