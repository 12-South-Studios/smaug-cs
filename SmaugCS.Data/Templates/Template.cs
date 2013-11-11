using System.Collections.Generic;
using Realm.Library.Common;
using SmaugCS.Common;

namespace SmaugCS.Data.Templates
{
    public abstract class Template : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public long Vnum { get; set; }
        public ExtendedBitvector ProgTypes { get; set; }
        public List<MudProgData> MudProgs { get; set; }

        protected Template(long id, string name)
            : base(id, name)
        {
            MudProgs = new List<MudProgData>();
        }

        public bool HasProg(int prog)
        {
            return ProgTypes.IsSet(prog);
        }
    }
}
