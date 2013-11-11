using System;
using SmaugCS.Data.Instances;

namespace SmaugCS.Data
{
    public class DoFunction
    {
        public Action<CharacterInstance, string> Value { get; set; }
    }
}
