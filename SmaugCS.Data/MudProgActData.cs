using System.Collections.Generic;
using SmaugCS.Data;

namespace SmaugCS.Data
{
    public class MudProgActData
    {
        public string buf { get; set; }
        public List<CharacterInstance> ch { get; set; }
        public List<ObjectInstance> obj { get; set; }
        public object vo { get; set; }
    }
}
