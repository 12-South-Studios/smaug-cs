using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;

namespace SmaugCS.Objects
{
    public class mpsleep_data
    {
        public RoomTemplate room { get; set; }
        public int timer { get; set; }
        public MudProgBaseTypes type { get; set; }
        public int ignorelevel { get; set; }
        public int iflevel { get; set; }
        public bool[,] ifstate { get; set; }
        public string com_list { get; set; }
        public CharacterInstance mob { get; set; }
        public CharacterInstance actor { get; set; }
        public ObjectInstance @object { get; set; }
        public CharacterInstance victim { get; set; }
        public object vo { get; set; }
        public bool single_step { get; set; }

        public mpsleep_data()
        {
            ifstate = new bool[Program.MAX_IFS, Program.DO_ELSE + 1];
        }
    }
}
