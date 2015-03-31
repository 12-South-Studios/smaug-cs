using System.Collections.Generic;

namespace SmaugCS.Objects
{
    public class mob_prog_act_list
    {
        public string buf { get; set; }
        public List<CharacterData> ch { get; set; }
        public List<ObjectData> obj { get; set; }
        public object vo { get; set; }
    }
}
