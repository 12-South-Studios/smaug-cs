using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;

namespace SmaugCS;

public class ExtracedCharacterData
{
    public CharacterInstance Character { get; set; }
    public RoomTemplate Room { get; set; }
    public ReturnTypes ReturnCode { get; set; }
    public bool Extract { get; set; }
}