using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Objects
{
    public class ExtracedCharacterData
    {
        public CharacterInstance Character { get; set; }
        public RoomTemplate Room { get; set; }
        public ReturnTypes ReturnCode { get; set; }
        public bool Extract { get; set; }
    }
}
