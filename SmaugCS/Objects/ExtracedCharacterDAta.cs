using SmaugCS.Data;
using SmaugCS.Data;

namespace SmaugCS.Objects
{
    public class ExtracedCharacterData
    {
        public CharacterInstance Character { get; set; }
        public RoomTemplate room { get; set; }
        public int retcode { get; set; }
        public bool extract { get; set; }
    }
}
