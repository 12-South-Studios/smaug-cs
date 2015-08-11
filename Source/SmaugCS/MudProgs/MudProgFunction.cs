using System;
using SmaugCS.Constants.Enums;

namespace SmaugCS.MudProgs
{
    public class MudProgFunction
    {
        public MudProgTypes Type { get; set; }
        public Func<object[], bool> Function { get; set; }
    }
}
