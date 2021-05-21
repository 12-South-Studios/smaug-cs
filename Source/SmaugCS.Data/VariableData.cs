using SmaugCS.Constants.Enums;
using System;

namespace SmaugCS.Data
{
    public class VariableData
    {
        public VariableTypes Type { get; set; }
        public int Flags { get; set; }
        public int vnum { get; set; }
        public DateTime CTime { get; set; }
        public DateTime MTime { get; set; }
        public DateTime RTime { get; set; }
        public DateTime expires { get; set; }
        public int Timer { get; set; }
        public string Tag { get; set; }
        public object Data { get; set; }
    }
}
