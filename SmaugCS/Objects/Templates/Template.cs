
using System.Collections.Generic;
using SmaugCS.Common;

// ReSharper disable CheckNamespace
namespace SmaugCS.Objects
// ReSharper restore CheckNamespace
{
    public abstract class Template
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int Vnum { get; set; }
        public ExtendedBitvector ProgTypes { get; set; }
        public List<MudProgData> MudProgs { get; set; }

        public bool HasProg(int prog)
        {
            return ProgTypes.IsSet(prog);
        }
    }
}
