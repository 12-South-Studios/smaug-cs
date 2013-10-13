
// ReSharper disable CheckNamespace

using System.Collections.Generic;

namespace SmaugCS.Objects
// ReSharper restore CheckNamespace
{
    public abstract class Instance
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        public Template Parent { get; set; }
        public List<AffectData> Affects { get; set; }

        public int Timer { get; set; }

        public Instance()
        {
            Affects = new List<AffectData>();
        }
    }
}
