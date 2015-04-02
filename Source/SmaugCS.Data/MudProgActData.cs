using System.Collections.Generic;
using SmaugCS.Data.Instances;

namespace SmaugCS.Data
{
    public class MudProgActData
    {
        public string buf { get; set; }
        public ICollection<CharacterInstance> ch { get; private set; }
        public ICollection<ObjectInstance> obj { get; private set; }
        public object vo { get; set; }
    }
}
